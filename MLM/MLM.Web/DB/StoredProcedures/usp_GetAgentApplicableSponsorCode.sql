--usp_GetAgentApplicableSponsorCode '2015030101', 1
USE [MLM]
GO
/****** Object:  StoredProcedure [dbo].[usp_AgentValidateJoining]    Script Date: 3/7/2015 1:04:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[usp_GetAgentApplicableSponsorCode]
	@ProvidedSponsorCode	 varchar(50), 
	@Position int -- 1 for Left and 2 for Right
as

declare @PositionStatus varchar(50)


BEGIN TRY
	DECLARE @Code int, @rightAgent nvarchar(max), @LeftAgent nvarchar(max)

	select @Code = Code,@rightAgent  = RightAgent, @LeftAgent = LeftAgent  from Agent where Code = @ProvidedSponsorCode

	-- Raise error if provided sponsor code is not valid
	IF @Code IS NULL
		Select '0' as AgentApplicableSponsorCode
		--RAISERROR ('Invalid sponsor code', 16, 1);

	-- Return same provided sponsor code as applicable code when provided position is direct null
	If ( (@Position = 1 and @LeftAgent IS NULL) OR ( @Position = 2 and @rightAgent IS NULL))
	BEGIN
		Select @ProvidedSponsorCode as AgentApplicableSponsorCode
	END
	ELSE
	BEGIN
	if (@Position = 1) 
		select dbo.ufn_AgentLeftDepthCode(@ProvidedSponsorCode) as AgentApplicableSponsorCode
	else
		select  dbo.ufn_AgentRightDepthCode(@ProvidedSponsorCode) as AgentApplicableSponsorCode
	END			

END TRY
BEGIN CATCH
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
 
