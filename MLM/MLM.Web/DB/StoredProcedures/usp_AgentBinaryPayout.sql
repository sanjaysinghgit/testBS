


-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\Shobhit\AppData\Local\Temp\~vs8A21.sql
ALTER proc [dbo].[usp_AgentBinaryPayout]
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
@SaveIncomeAmount int,
@PVAmount decimal(18,2),
@pairincome decimal (18,2),
@TotalIncome decimal (18,2)
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
select @VoucherStartDate = (Select Top 1 VoucherDate  from Payout order by VoucherDate Desc)
select @VoucherStartDate = isnull(@VoucherStartDate, cast('' as datetime))
    
    
    
    --===============Get Save Income Amount==========
    
    declare @TotalSaveIncomeMemberInVoucherDuration int,
			-- got from setting @SaveIncomeAmount decimal (18,2),
			@TotalSaveIncomeMemberTillValid int
			
		Select @TotalSaveIncomeMemberInVoucherDuration  = Count(*) from Agent 
				where Status = 4 and ActivationDate between @VoucherStartDate and @VoucherEndDate
			
			Select @TotalSaveIncomeMemberTillValid = Count(*) from Agent 
					where VoucherStatus = 1 and Status = 4 and SaveIncomeStatus = 1
			
    
    --================================================
set @Panno = 'AS23G567H3'

--set @agentpaidpairsold = 0

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
		--set @tdsamount = 0

		--CASE 1#: Get settings 

		--CASE 2#: If binary type is of Pair then follow CASE 3# else CASE 4#


		-- CASE 3#: Pair Income caluclation
		BEGIN
			--Get Agent Paid Pairs
			set @agentpaidpairsold = (select isnull(SUM(PairsInThisPayout),0) from Payout where AgentCode = @agentcode )
		
			--TODO: Replace function.
			--select Total Left/Right Agents
			select @countleft = TotLeft,@countright = TotRight from ufn_AgentTotalLeftRight(@agentcode)

			
       
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
			if(@pairincome > @MonthlyBinaryCapping)
				Begin
				set @pairincome=@MonthlyBinaryCapping
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
			
			@PerPersonSaveIncome  decimal (18,2),
			@JoiningAmount decimal (18,2),
			@SumOfOldGivenSaveIncome decimal (18,2),
			@ApplicableSaveIncomeToBeGiven decimal (18,2)
			
			set @SaveIncome =0.0
			set @PerPersonSaveIncome = 0.0
			
			--=================================== SAVE Income Formula:   
			-- PerPersonSaveIncome = TotalSaveIncomeMemberInVoucherDuration*SaveIncomeAmount(Taken extra at the time of joining) / TotalSaveIncomeMemberTillValid
			-- ApplicableSaveIncomeToBeGiven = JoiningAmount - SumOfOldGivenSaveIncome
			-- PerPersonSaveIncome > ApplicableSaveIncomeToBeGiven ? PerPersonSaveIncome - ApplicableSaveIncomeToBeGiven (And mark person Invalid ) : PerPersonSaveIncome

			
			--#Formula step 1:
			declare @agentsaveincomestatus bit
			select @agentsaveincomestatus = SaveIncomeStatus from agent where code = @agentcode 
			
			if (@agentsaveincomestatus = 1)
			begin
						if @TotalSaveIncomeMemberTillValid > 0
						begin
							Set @PerPersonSaveIncome = @TotalSaveIncomeMemberInVoucherDuration * @SaveIncomeAmount / @TotalSaveIncomeMemberTillValid
						-- JoiningAmount from product table, for now its fixed.
						-- TODO:????
								Set @JoiningAmount = 3200
								
								Select @SumOfOldGivenSaveIncome = isnull(sum(SaveIncome),0) from Payout 
									where Agentcode = @agentcode

								--#Formula step 2:
								set @ApplicableSaveIncomeToBeGiven = @JoiningAmount - @SumOfOldGivenSaveIncome

								--#Formula step3:
								IF(@PerPersonSaveIncome > @ApplicableSaveIncomeToBeGiven)
								BEGIN
									set @SaveIncome =  @ApplicableSaveIncomeToBeGiven
									Update Agent set SaveIncomeStatus = 0 where code = @agentcode
								END
								ELSE
								BEGIN
									set @SaveIncome = @PerPersonSaveIncome
								END
						End
						
			End
			else
			begin
				set @SaveIncome  = 0
				
			End
			set @TotalIncome =(isnull(@totalComm,0) + @SaveIncome)
		END
		-- END CASE 5#
			
		--===========================================================
		-- Check Pan No & TDS According to Pan Number
		--===========================================================
		if (len(@Panno)<>10)
		   begin
					set @tdsamount=(@TDSWithoutPan*@TotalIncome)/100
		   end
		else
		   begin
					set @tdsamount=(@TDSWithPan*@TotalIncome)/100
		   end
		-- set @hc=((select hc from tbl_taxes)*(@pairincome+@renewalpairincometotal))/100
		set @processingchargeamount=(@TotalIncome*@processingchargePer)/100
		if (@processingchargeamount > @processingchargeCapping )
			begin
			set @processingchargeamount=@processingchargeCapping
			end
		set @dispatchedamount=@TotalIncome-(@tdsamount+@processingchargeamount)
		
		
		insert into payout(VoucherDate,agentcode,Pan,TotalLeftPair,TotalRightPair,
		 PairsInThisPayout,SaveIncome,PairIncome,tds,
		 processingcharges,NetIncome,
		 DispatchedAmount,CreatedDate,UpdateDate,TotalLeftPairPV,TotalRightPairPV,
		 PairsPVInThisPayout,IsDeleted)
			values(getDate(),@agentcode,@Panno,@countleft,@countright, 
			@agentpaidpairsnew,@SaveIncome,isnull(@totalComm,0),isnull(@tdsamount,0),
			isnull(@processingchargeamount,0),isnull(@TotalIncome,0),
			isnull(@dispatchedamount,0),GETDATE(),GETDATE(),0,0,0,0)
   
   
   
   --========================Pair Wise & Time Wise Prize=============================
   Declare @JoiningDays int,
   @prize int
   set @Prize = 0
   
   select @JoiningDays = datediff(day,ActivationDate,@VoucherEndDate) from agent where code = @agentcode
   
   
   --TO Do
  -- select *  PairPriceMaster where pair => @countpairs and pair < @countpairs
set  @Prize = case when  @countpairs >= 5 and @countpairs <9 and @JoiningDays<= 15 then  700
   
		when @countpairs >= 10 and @countpairs <19 and @JoiningDays<= 30  then  1500
   
		when @countpairs >= 20 and @countpairs <34 and @JoiningDays<= 60  then 3000
		when @countpairs >= 35 and @countpairs <69 and @JoiningDays<= 90  then  5000
   		when @countpairs >= 70 and @countpairs <99 and @JoiningDays<= 120 then  11000
   		when @countpairs >= 100 and @countpairs <149 and @JoiningDays<= 150 then  11000
   		else 0 end 
   		
   		if @prize >0
   		begin
   		insert into PairPrizeWinner (voucherid,voucherdate,Agentcode,TotPair,days,prize) 
   		values(1,@VoucherEndDate,@agentcode,@countpairs,isnull(@JoiningDays,0),isnull(@Prize,0))
		end
    
   --===================================End==================================================
   
	
			fetch  _agentcursor into @agentcode
		end
		close _agentcursor;
		deallocate _agentcursor;

END





