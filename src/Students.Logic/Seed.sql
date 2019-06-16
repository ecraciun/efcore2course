GO
SET IDENTITY_INSERT [dbo].[Courses] ON

GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (1, N'Calculus', 3)
GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (2, N'Chemistry', 3)
GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (3, N'Composition', 3)
GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (4, N'Literature', 4)
GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (5, N'Trigonometry', 4)
GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (6, N'Microeconomics', 3)
GO
INSERT [dbo].[Courses] ([Id], [Name], [Credits]) VALUES (7, N'Macroeconomics', 3)
GO
SET IDENTITY_INSERT [dbo].[Courses] OFF
GO



SET IDENTITY_INSERT [dbo].[Students] ON 
GO
INSERT [dbo].[Students] ([Id], [Name], [Email]) VALUES (1, N'Alice', N'alice@gmail.com')
GO
INSERT [dbo].[Students] ([Id], [Name], [Email]) VALUES (2, N'Bob', N'bob@outlook.com')
GO
SET IDENTITY_INSERT [dbo].[Students] OFF
GO


GO
SET IDENTITY_INSERT [dbo].[Enrollments] ON 
GO
INSERT [dbo].[Enrollments] ([Id], [StudentID], [CourseID], [Grade]) VALUES (1, 2, 2, 1)
GO
INSERT [dbo].[Enrollments] ([Id], [StudentID], [CourseID], [Grade]) VALUES (2, 2, 3, 3)
GO
INSERT [dbo].[Enrollments] ([Id], [StudentID], [CourseID], [Grade]) VALUES (3, 1, 1, 1)
GO
INSERT [dbo].[Enrollments] ([Id], [StudentID], [CourseID], [Grade]) VALUES (4, 1, 2, 3)
GO
SET IDENTITY_INSERT [dbo].[Enrollments] OFF
