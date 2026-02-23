CREATE DATABASE OfimedicDB;
GO

USE OfimedicDB;
GO

CREATE TABLE Albums (
    Id INT PRIMARY KEY,
    UserId INT,
    Title NVARCHAR(200)
);

CREATE TABLE Photos (
    Id INT PRIMARY KEY,
    AlbumId INT,
    Title NVARCHAR(200),
    Url NVARCHAR(500),
    ThumbnailUrl NVARCHAR(500)
);