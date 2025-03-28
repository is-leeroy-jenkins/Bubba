CREATE TABLE IF NOT EXISTS "Appropriations" 
(
	"AppropriationsId"	INTEGER NOT NULL UNIQUE,
	"FiscalYear"	TEXT(80) DEFAULT 'NS',
	"PublicLaw"	TEXT(80) DEFAULT 'NS',
	"AppropriationTitle"	TEXT(80) DEFAULT 'NOT SPECIFIED',
	"EnactedDate"	TEXT(80) DEFAULT 'NS',
	"ExplanatoryComments"	TEXT(80) DEFAULT 'NOT SPECIFIED',
	"Authority"	DOUBLE DEFAULT 0.0,
	PRIMARY KEY("AppropriationsId" AUTOINCREMENT)
);