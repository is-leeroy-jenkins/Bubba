CREATE TABLE IF NOT EXISTS BudgetObjectClasses
(
	BudgetObjectClassesId INTEGER NOT NULL UNIQUE,
	Code TEXT(150) NULL DEFAULT NS,
	Name TEXT(150) NULL DEFAULT NS,
	CONSTRAINT BudgetObjectClassesPrimaryKey 
		PRIMARY KEY(BudgetObjectClassesId)
);
