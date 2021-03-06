-- [usp_AgentSponsorRegistrationConditions] '2015030101', '1900010101', '1900010101', 1
/****** Object:  StoredProcedure [dbo].[CheckjoinConditions]    Script Date: 03/05/2015 01:40:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery1.sql|7|0|C:\Users\Shobhit\AppData\Local\Temp\~vsEF8B.sql
alter PROCEDURE [dbo].[usp_AgentSponsorRegistrationConditions]
	@Agentcode varchar(50),
	@SponsorCode	 varchar(50), --Calculated by system
	@IntroducerCode	 varchar(50), --Provided by User
	@Position int
as

declare @PositionStatus varchar(50)

BEGIN TRY
	BEGIN TRANSACTION MAIN
	-- 1. Update sponsor's left or right positions
		if (@Position = 1) 
			begin
						update Agent set LeftAgent  = @Agentcode, UpdateDate=GETUTCDATE()  where Code = @SponsorCode
			end
		else
			begin
					update Agent set RightAgent  = @Agentcode, UpdateDate=GETUTCDATE()  where Code = @SponsorCode
			end
			
	-- 2. Update Status Save Income
			Declare @SaveIncomeDaysDuration int
			select @SaveIncomeDaysDuration = SaveIncomeDaysDuration from Setting
			 

			if((select DATEDIFF(day,(select activationDate from Agent where Code = @IntroducerCode), activationDate) from Agent where Code = @Agentcode) <= @SaveIncomeDaysDuration)
			begin
				
				-- as of now its is hard coded for at least 2 joining.
				-- We can change it to something like completion of left and right pair.
				if( (select COUNT(*) from Agent where IntroducerCode = @IntroducerCode) >= 2)
				begin
				update Agent set SaveIncomeStatus  = 1, UpdateDate=GETUTCDATE()  where Code = @IntroducerCode
				end
				
			end

	--3. Update Voucher Status
			
			if((select VoucherStatus from Agent where Code = @IntroducerCode) = 0)
				begin
				if( (select COUNT(*) from Agent where IntroducerCode = @IntroducerCode) >= 2)
					begin
					update Agent set VoucherStatus  = 1, UpdateDate=GETUTCDATE()  where Code = @IntroducerCode
					end
				end
	Commit Transaction MAIN

end TRY
BEGIN CATCH
	ROLLBACK TRANSACTION MAIN
DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH
