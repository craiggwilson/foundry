USE [FoundryEvents]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EventStore_GeneratedOn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EventStore] DROP CONSTRAINT [DF_EventStore_GeneratedOn]
END

GO

USE [FoundryEvents]
GO

/****** Object:  Table [dbo].[EventStore]    Script Date: 09/04/2010 23:40:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventStore]') AND type in (N'U'))
DROP TABLE [dbo].[EventStore]
GO

/****** Object:  Table [dbo].[SnapshotStore]    Script Date: 09/04/2010 23:40:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SnapshotStore]') AND type in (N'U'))
DROP TABLE [dbo].[SnapshotStore]
GO

USE [FoundryEvents]
GO

/****** Object:  Table [dbo].[EventStore]    Script Date: 09/04/2010 23:40:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EventStore](
	[StreamId] [uniqueidentifier] NOT NULL,
	[Sequence] [int] IDENTITY(1,1) NOT NULL,
	[GeneratedOn] [datetime] NOT NULL,
	[EventData] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_EventStore] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [FoundryEvents]
GO

/****** Object:  Table [dbo].[SnapshotStore]    Script Date: 09/04/2010 23:40:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SnapshotStore](
	[SourceId] [uniqueidentifier] NOT NULL,
	[SnapshotData] [varbinary](max) NOT NULL,
	[version] [timestamp] NOT NULL,
 CONSTRAINT [PK_SnapshotStore] PRIMARY KEY CLUSTERED 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[EventStore] ADD  CONSTRAINT [DF_EventStore_GeneratedOn]  DEFAULT (getdate()) FOR [GeneratedOn]
GO


