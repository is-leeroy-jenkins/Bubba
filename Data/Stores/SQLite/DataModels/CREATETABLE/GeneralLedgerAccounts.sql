CREATE TABLE IF NOT EXISTS GeneralLedgerAccounts
(
	GeneralLedgerAccountsId INTEGER NOT NULL UNIQUE,
	BFY TEXT(150) NULL DEFAULT NS,
	Number TEXT(150) NULL DEFAULT NS,
	Name TEXT(150) NULL DEFAULT NS,
	ShortName TEXT(150) NULL DEFAULT NS,
	AccountClassification TEXT(150) NULL DEFAULT NS,
	NormalBalance TEXT(150) NULL DEFAULT NS,
	RealOrClosingAccount TEXT(150) NULL DEFAULT NS,
	CashAccount TEXT(150) NULL DEFAULT NS,
	SummaryAccount TEXT(150) NULL DEFAULT NS,
	ReportableAccount TEXT(150) NULL DEFAULT NS,
	CostAllocationIndicator TEXT(150) NULL DEFAULT NS,
	FederalNonFederal TEXT(150) NULL DEFAULT NS,
	AttributeValue TEXT(150) NULL DEFAULT NS,
	Usage TEXT(150) NULL DEFAULT NS,
	Deposit TEXT(150) NULL DEFAULT NS,
	CONSTRAINT GeneralLedgerAccountsPrimaryKey) PRIMARY KEY(GeneralLedgerAccountsId)
);
