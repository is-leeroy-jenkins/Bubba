CREATE TABLE IF NOT EXISTS "ResourceLines" 
	"ReportingLinesId" INTEGER NOT NULL UNIQUE,
	"Number" TEXT(80) DEFAULT 'NS',
	"Name" TEXT(80) DEFAULT 'NS',
	"Caption" TEXT(80) DEFAULT 'NS',
	"Category" TEXT(80) DEFAULT 'NS',
	"Range"	TEXT(80) DEFAULT 'NS',
	PRIMARY KEY("ReportingLinesId" AUTOINCREMENT)
);