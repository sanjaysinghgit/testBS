DROP Function [dbo].[ufn_AgentRightDepthCode]
GO

/****** Object:  UserDefinedFunction [dbo].[DeptRightAgentFun]    Script Date: 03/05/2015 01:43:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Function [dbo].[ufn_AgentRightDepthCode]
	(@AgentCode	 varchar(50))
	
	returns varchar(50)
as

begin

declare @DRightCode varchar(50);
declare @DownRightagent varchar(50)
set @DownRightagent = (select RightAgent from Agent where code = @AgentCode);

WITH TreeReport (AgentCode,SponsorCode,IntroducerCode,Position,Level)
AS
(
	--Create base record T(0)
    SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,0 AS Level
		FROM Agent AS a where a.Code=@DownRightagent		
    UNION ALL 
	-- Recurse till T(n)
	SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,
		Level + 1
		FROM Agent AS a 
		INNER JOIN TreeReport AS d ON a.SponsorCode = d.AgentCode and a.Position = 2
)

---- Executes the CTE

 select @DRightCode =(SELECT AgentCode FROM TreeReport where Level = (select MAX(level) from TreeReport ))

return @DRightCode
end	