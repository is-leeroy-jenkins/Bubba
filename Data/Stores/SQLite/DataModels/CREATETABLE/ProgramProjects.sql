CREATE TABLE IF NOT EXISTS ProgramProjects
(
	ProgramProjectsId INTEGER NOT NULL UNIQUE,
	Code TEXT(150) NULL DEFAULT NS,
	Name TEXT(150) NULL DEFAULT NS,
	ProgramAreaCode TEXT(150) NULL DEFAULT NS,
	ProgramAreaName TEXT(150) NULL DEFAULT NS,
	CONSTRAINT ProgramProjectsPrimaryKey 
		PRIMARY KEY(ProgramProjectsId)
);
