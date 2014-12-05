--Update tables structure

ALTER TABLE UserProfile ADD [Username] [nvarchar](max) NULL
ALTER TABLE UserProfile ADD [OrganisationOrGroup] [nvarchar](max) NULL
ALTER TABLE UserProfile ADD [GroupAddress] [nvarchar](max) NULL
ALTER TABLE UserProfile ADD [City] [nvarchar](max) NULL
ALTER TABLE UserProfile ADD [State] [int] NOT NULL DEFAULT 0
ALTER TABLE UserProfile ADD [Postcode] [nvarchar](max) NULL
ALTER TABLE UserProfile ADD [Phone] [nvarchar](max) NULL
ALTER TABLE UserProfile ADD [AboutGroup] [nvarchar](max) NULL

GO
-->Update structure ended.






