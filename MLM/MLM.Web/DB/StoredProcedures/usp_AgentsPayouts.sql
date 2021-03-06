
create proc [dbo].[usp_AgentsPayouts]
@VoucherStartDate datetime,
@VoucherEndDate datetime
AS
BEGIN

	select @VoucherStartDate = isnull(@VoucherStartDate, cast('' as datetime))
    
	select VoucherDate,agentcode,Pan,TotalLeftPair,TotalRightPair,
		PairsInThisPayout,SaveIncome,PairIncome,tds,
		processingcharges,NetIncome,
		DispatchedAmount,CreatedDate,UpdateDate,TotalLeftPairPV,TotalRightPairPV,
		PairsPVInThisPayout from payout
	where isDeleted = 0 and createdDate between @VoucherStartDate and @VoucherEndDate
END
