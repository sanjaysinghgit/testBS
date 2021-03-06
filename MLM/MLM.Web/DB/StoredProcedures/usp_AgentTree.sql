--[usp_AgentTree] '1900010101'

/****** Object:  StoredProcedure [dbo].[usp_AgentTree]    Script Date: 3/30/2015 11:43:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_AgentTree]
	@AgentCode	 varchar(50) 
as
begin

WITH TreeReport (AgentCode,SponsorCode,IntroducerCode,Position,Status, Id)
AS
(
	--Create base record T(0)
    SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position, a.Status, a.Id
		FROM Agent AS a where a.Code=@AgentCode		
    UNION ALL 
	-- Recurse till T(n)
	SELECT a.Code,a.SponsorCode,a.IntroducerCode,a.Position,
		a.Status, a.Id
		FROM Agent AS a 
		INNER JOIN TreeReport AS d ON a.IntroducerCode = d.AgentCode
)

---- Executes the CTE

SELECT 'SrNo' = ROW_NUMBER() OVER(ORDER BY AgentCode asc ),
	t.AgentCode,t.SponsorCode,t.IntroducerCode,t.Position,t.status, u.Name FROM TreeReport t
	Join Users u on t.Id = u.AgentInfo_Id
end