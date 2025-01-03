CREATE TABLE IF NOT EXISTS AppropriationAvailableBalances
(
	AppropriationAvailableBalancesId INTEGER NOT NULL UNIQUE,
	BFY TEXT(150) NULL DEFAULT NS,
	EFY TEXT(150) NULL DEFAULT NS,
	FundCode TEXT(150) NULL DEFAULT NS,
	FundName TEXT(150) NULL DEFAULT NS,
	BudgetAccountCode TEXT(150) NULL DEFAULT NS,
	BudgetAccountName TEXT(150) NULL DEFAULT NS,
	TreasuryAccountCode TEXT(150) NULL DEFAULT NS,
	TreasuryAccountName TEXT(150) NULL DEFAULT NS,
	OriginalAmount DOUBLE NULL DEFAULT 0.0,
	Authority DOUBLE NULL DEFAULT 0.0,
	Budgeted DOUBLE NULL DEFAULT 0.0,
	Posted DOUBLE NULL DEFAULT 0.0,
	CarryoverIn DOUBLE NULL DEFAULT 0.0,
	CarryoverOut DOUBLE NULL DEFAULT 0.0,
	Used DOUBLE NULL DEFAULT 0.0,
	Available DOUBLE NULL DEFAULT 0.0,
	CONSTRAINT AppropriationAvailableBalancesPrimaryKey 
		PRIMARY KEY(AppropriationAvailableBalancesId)
);
