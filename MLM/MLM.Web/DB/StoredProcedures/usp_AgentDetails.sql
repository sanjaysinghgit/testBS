

ALTER PROCEDURE [dbo].[usp_AgentDetails]
	@AgentCode	 varchar(50)
as
begin
select(
select [dbo].[ufn_AgentTotalLeft](@AgentCode)) as leftcount,
			(select [dbo].[ufn_AgentTotalRight](@AgentCode))as rightcount

end
