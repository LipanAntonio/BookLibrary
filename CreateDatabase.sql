CREATE DATABASE LibraryDB

USE LibraryDB

CREATE TABLE Books (
    ID INT PRIMARY KEY IDENTITY (1, 1),
    Name VARCHAR (50) NOT NULL,
    Author VARCHAR (50) NOT NULL,
	Stock int);

INSERT INTO Books (Name, Author, Stock)
VALUES ('The Lord of the Rings', 'J. R. R. Tolkien', 14), ('Fahrenheit 451', 'Ray Bradbury', 2), ('Don Quixote', 'Migues de Cervantes', 6)

CREATE TABLE Clients (
    ID INT PRIMARY KEY IDENTITY (1, 1),
    Name VARCHAR (50) NOT NULL,
    Email VARCHAR (50) NOT NULL,
	BookRented VARCHAR (50) NOT NULL);
	
CREATE TABLE RentedBooks (
    ID INT PRIMARY KEY IDENTITY (1, 1),
    Name VARCHAR (50) NOT NULL,
    Author VARCHAR (50) NOT NULL,
	Renter VARCHAR (50) NOT NULL);

	select * from Books
	select * from Clients
	select * from RentedBooks

