SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\Shobhit\AppData\Local\Temp\~vs8A21.sql
Create proc [dbo].[usp_AgentBinaryPayout]
--@startdate datetime , Get automaticlly from last voucher date
@VoucherEndDate datetime
AS
 BEGIN


Declare

@countleft int,
@countright int,
@Panno varchar(10) ,
@agentname varchar(50),
@countpairs int,
@agentpaidpairsnew int,
@agentpaidpairsold int,
@PairIncomeAmout int,
@WeeklyBinaryCapping bigint,
@cappingdays int,
@totalComm decimal(18,2),
@TDSWithPAN decimal(18,2),
@TDSWithOutPAN decimal(18,2) ,
@tdsamount int,
@processingchargePer decimal (18,2),
@processingchargeamount bigint,
@processingchargeCapping bigint,
@dispatchedamount decimal (18,2),
@MonthlyBinaryCapping bigint,
@WeeklyRepurchaseCapping bigint,
@MonthlyRepurchaseCapping bigint,
@SaveIncomeDaysDuration int,
@SaveIncomeAmount decimal(18,2),
@PVAmount decimal(18,2)
--@agentname varchar(50)


--insert into tbl_WeeklyPayouts values(0.00,@startdate,@enddate,convert(varchar(10),getdate(),101),0.00,0.00,0.00,0.00,'WEEKLY')
-----print (@@identity)
--set @Payoutno = @@identity
select  @TDSWithPAN = TDSWithPAN,
		@TDSWithOutPAN = TDSWithOutPAN ,
		@processingchargePer=VoucherProcessingCharge,
		@processingchargeCapping =VoucherProcessingChargeCapping,
		@PairIncomeAmout= PairIncomeAmout,
		@WeeklyBinaryCapping = WeeklyBinaryCapping ,
		@MonthlyBinaryCapping =MonthlyBinaryCapping,
		@WeeklyRepurchaseCapping = WeeklyRepurchaseCapping,
		@MonthlyRepurchaseCapping = MonthlyRepurchaseCapping,
		@SaveIncomeAmount = SaveIncomeAmount,
		@SaveIncomeDaysDuration = SaveIncomeDaysDuration,
		@PVAmount = PVAmount
    from Setting 

Declare @VoucherStartDate Datetime 
Select Top 1 @VoucherStartDate = VoucherDate  from Payout order by VoucherDate Desc
select isnull(@VoucherStartDate, cast('' as datetime))
    
    
set @Panno = 'AS23G567H3'
--set @TDSWithPan = 10
--set @TDSWithoutPan = 20
--set @processingchargePer =0.0
--set @processingchargeCapping = 0
--set @PairIncomeAmout = 200
set @agentpaidpairsold = 0
--set @Weeklycappingamount = 20000

	-- Voucher status is being set when it completes self branch. This is required to have voucher
	-- After being user of system, person becomes Guest&Active and after buying complete Joining amout system makes user
	-- as Agent&Active. These two are required for Voucher. Guests are not required for Binary Income.
	Declare _agentcursor cursor for select code from agent  where VoucherStatus = 1 and status = 4
	open _agentcursor

	declare @agentcode nvarchar(100)
	fetch _agentcursor into @agentcode
	--LOOP all eligible agents
	while(@@fetch_status=0)
		begin
		set @tdsamount = 0

		--CASE 1#: Get settings 

		--CASE 2#: If binary type is of Pair then follow CASE 3# else CASE 4#


		-- CASE 3#: Pair Income caluclation
		BEGIN
			set @agentpaidpairsold = 0
			set @agentpaidpairsold = (select SUM(PairsInThisPayout) from Payout where AgentCode = @agentcode )
		
			--TODO: Replace function.
			--select @countleft= dbo.Fun_Agenttotalleft(@agentcode)
			set  @countleft = 10

			--TODO: Replace function.
			--select @countright= dbo.Fun_Agenttotalright(@agentcode)
			set @countright= 10
       
			--set @agentname=(select name from users where agentinfo_Id=(select id from Agent where code = @agentcode ))
			if(@countleft<@countright)
				begin
					set @countpairs=@countleft
				end
			else
				begin
				set @countpairs=@countright
				end
       
			   --set @agentpaidpairsnew=@countpairs-(select isnull(totalpaidpairs,0) from tbl_agent where agentcode=@agentcode)

		   set @agentpaidpairsnew = @countpairs - @agentpaidpairsold 
       
       
			if(@agentpaidpairsnew > 0)
			begin
			set @pairincome=@agentpaidpairsnew*@PairIncomeAmout

			-- TODO: FOr now Assuming @incomecappingamount is monthly, and we are prepairing voucher monthly.
			-- Or @incomecappingamount is weekly , and we are prepairing voucher weekly. THen it will work fine.
			-- In case of weekly capping with monthly voucher: NEED TO PUT THIS logic.
			if(@pairincome > @incomecappingamount)
				Begin
				set @pairincome=@incomecappingamount
				End
			end
           
			set @totalComm=@pairincome	
		END
		-- END CASE 3#: Pair Income caluclation

		-- CASE 4# PV Income calculation
		-- TOD0: NEED to do

		--CASE 5# Save income calculation
		BEGIN
			
			Declare 
			@SaveIncome decimal (18,2),
			@TotalSaveIncomeMemberInVoucherDuration int,
			-- got from setting @SaveIncomeAmount decimal (18,2),
			@TotalSaveIncomeMemberTillValid int,
			@PerPersonSaveIncome  decimal (18,2),
			@JoiningAmount decimal (18,2),
			@SumOfOldGivenSaveIncome decimal (18,2),
			@ApplicableSaveIncomeToBeGiven decimal (18,2)

			-- SAVE Income Formula:   
			-- PerPersonSaveIncome = TotalSaveIncomeMemberInVoucherDuration*SaveIncomeAmount(Taken extra at the time of joining) / TotalSaveIncomeMemberTillValid
			-- ApplicableSaveIncomeToBeGiven = JoiningAmount - SumOfOldGivenSaveIncome
			-- PerPersonSaveIncome > ApplicableSaveIncomeToBeGiven ? PerPersonSaveIncome - ApplicableSaveIncomeToBeGiven (And mark person Invalid ) : PerPersonSaveIncome

			Select @TotalSaveIncomeMemberInVoucherDuration  = Count(*) from Agent 
				where Status = 4 and ActivationDate between @VoucherStartDate and @VoucherEndDate
			
			Select @TotalSaveIncomeMemberTillValid = Count(*) from Agent 
					where VoucherStatus = 1 and Status = 4 and SaveIncomeStatus = 1
			
			--#Formula step 1:
			Set @PerPersonSaveIncome = @TotalSaveIncomeMemberInVoucherDuration * @SaveIncomeAmount / @TotalSaveIncomeMemberTillValid
			

			-- JoiningAmount from product table, for now its fixed.
			-- TODO:????
			Set @JoiningAmount = 1500
			
			Select @SumOfOldGivenSaveIncome = sum(SaveIncome) from Payout where Agentcode = @agentcode

			--#Formula step 2:
			set @ApplicableSaveIncomeToBeGiven = @JoiningAmount - @SumOfOldGivenSaveIncome

			--#Formula step3:
			IF(@PerPersonSaveIncome > @ApplicableSaveIncomeToBeGiven)
			BEGIN
				set @SaveIncome = @PerPersonSaveIncome - @ApplicableSaveIncomeToBeGiven
				Update Agent set SaveIncomeStatus = 0 where code = @agentcode
			END
			ELSE
			BEGIN
				set @SaveIncome = @PerPersonSaveIncome
			END
		END
		-- END CASE 5#


		--===========================================================
		-- Check Pan No & TDS According to Pan Number
		--===========================================================
		if (len(@Panno)<>10)
		   begin
					set @tdsamount=(@TDSWithoutPan*(@totalComm + @SaveIncome))/100
		   end
		else
		   begin
					set @tdsamount=(@TDSWithPan*(@totalComm + @SaveIncome))/100
		   end
		-- set @hc=((select hc from tbl_taxes)*(@pairincome+@renewalpairincometotal))/100
		set @processingchargeamount=((@totalComm + @SaveIncome)*@processingchargePer)/100
		if (@processingchargeamount > @processingchargeCapping )
			begin
			set @processingchargeamount=@processingchargeCapping
			end
		set @dispatchedamount=(@totalComm + @SaveIncome)-(@tdsamount+@processingchargeamount)
		--print('dispatchedamount=')
		--print(@dispatchedamount)
		select * from payout
		insert into payout(VoucherDate,agentcode,Pan,TotalLeftPair,TotalRightPair, PairsInThisPayout,SaveIncome,TotalIncome,tds,processingcharges,NetIncome,DispatchedAmount)
			values(getDate(),@agentcode,@Panno,@countleft,@countright, @agentpaidpairsnew,@SaveIncome,@PairIncomeAmout,@tdsamount,@processingchargeamount,@dispatchedamount,@dispatchedamount)
   
   
		--   Declare @tpo int,@tprII int,@tprIII int
		--   set @tpo=(select isnull(totalpaidpairs,0) from tbl_agent where agentcode=@agentcode)+@agentpaidpairsnew
		--            ------------------set @tprII=(select isnull(renewalpaidpairsII,0) from tbl_agent where agentcode=@agentcode)+@agentrenewalpaidpairsII
		--            ------------------set @tprIII=(select isnull(renewalpaidpairsIII,0) from tbl_agent where agentcode=@agentcode)+@agentrenewalpaidpairsIII
		--            ------------------update tbl_agent set totalpaidpairs=@tpo,renewalpaidpairsII=@tprII,renewalpaidpairsIII=@tprIII where agentcode=@agentcode
		--   update tbl_agent set totalpaidpairs=@tpo where agentcode=@agentcode 
		--   print ('agentcodecountpairnew tpo=')
		--   print( @tpo)
		--    set @totalamountcounter=@totalamountcounter+(@totalearned)
		--print('totalamount=')
		--print(@totalamountcounter)     
		--           set @processingchargecounter=@processingchargecounter+@processingchargeagent
		--    --set @totalhc=@totalhc+@hc
                  
		--    set @totaltds=@totaltds+@tds
	   --end
			fetch  _agentcursor into @agentcode
		end
		close _agentcursor;
		deallocate _agentcursor;
--set @dispatchedamount=@totalamountcounter-(@totaltds+@processingchargecounter)

--update tbl_WeeklyPayouts set totalamount=@totalamountcounter,tds=@totaltds,dispatchedamount=@dispatchedamount,processingcharge=@processingchargecounter where id=@Payoutno
END