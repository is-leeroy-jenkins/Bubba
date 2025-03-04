CREATE TABLE IF NOT EXISTS Resources
(
	ResourcesId INTEGER NOT NULL UNIQUE,
	Identifier TEXT(150) NULL DEFAULT NS,
	Type TEXT(150) NULL DEFAULT NS,
	Location TEXT(150) NULL DEFAULT NS,
	FileExtension TEXT(150) NULL DEFAULT NS,
	Caption TEXT(150) NULL DEFAULT NS,
	CONSTRAINT ResourcesPrimaryKey 
		PRIMARY KEY(ResourcesId)
);
