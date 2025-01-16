-- Erstellen der Tabellen
CREATE TABLE Teacher (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Firstname VARCHAR(50) NOT NULL,
    Lastname VARCHAR(50) NOT NULL
);

CREATE TABLE Student (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Firstname VARCHAR(50) NOT NULL,
    Lastname VARCHAR(50) NOT NULL
);

CREATE TABLE Tag (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);

CREATE TABLE Exercise (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT NOT NULL,
    Language ENUM('CSharp', 'Java', 'TypeScript') NOT NULL,
    TeacherId INT NOT NULL,
    StudentId INT,
    TotalTests INT,
    PassedTests INT,
    FailedTests INT,
    DateCreated DATETIME NOT NULL,
    DateUpdated DATETIME NOT NULL,
    FOREIGN KEY (TeacherId) REFERENCES Teacher(Id) ON DELETE NO ACTION,
    FOREIGN KEY (StudentId) REFERENCES Student(Id) ON DELETE NO ACTION
);

CREATE TABLE ArrayOfSnippets (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ExerciseId INT NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES Exercise(Id) ON DELETE CASCADE
);

CREATE TABLE Snippet (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Code TEXT NOT NULL,
    ReadonlySection BOOLEAN NOT NULL,
    FileName VARCHAR(100) NOT NULL,
    ArrayOfSnippetsId INT NOT NULL,
    FOREIGN KEY (ArrayOfSnippetsId) REFERENCES ArrayOfSnippets(Id) ON DELETE CASCADE
);

CREATE TABLE Assignments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Link VARCHAR(255),
    DateDue DATETIME NOT NULL,
    ExerciseId INT NOT NULL,
    TeacherId INT NOT NULL,
    FOREIGN KEY (ExerciseId) REFERENCES Exercise(Id) ON DELETE NO ACTION,
    FOREIGN KEY (TeacherId) REFERENCES Teacher(Id) ON DELETE NO ACTION
);

CREATE TABLE AssignmentUser  (
    AssignmentId INT NOT NULL,
    StudentId INT NOT NULL,
    PRIMARY KEY (AssignmentId, StudentId),
    FOREIGN KEY (AssignmentId) REFERENCES Assignments(Id) ON DELETE CASCADE,
    FOREIGN KEY (StudentId) REFERENCES Student(Id) ON DELETE CASCADE
);

-- Einfügen der Daten in die Tabellen

-- Lehrer
INSERT INTO Teacher (Username, Firstname, Lastname) VALUES
('if200183', 'Florian', 'Hagmair');

-- Studenten
INSERT INTO Student (Username, Firstname, Lastname) VALUES
('if200182', 'David', 'Pröller'),
('if200104', 'Christian', 'Ekhator'),
('if200177', 'Samuel', 'Atzlesberger'),
('if200145', 'Michael', 'Werner'),
('if200107', 'Marcus', 'Rabeder');

-- Tags
INSERT INTO Tag (Name) VALUES
('Class1'), ('POSE'), ('WMC'), ('Class2'), ('SYP'), ('Class3');

-- Übungen
INSERT INTO Exercise (Name, Description, Language, TeacherId, StudentId, TotalTests, PassedTests, FailedTests, DateCreated, DateUpdated) VALUES
('Addition', 'Implement the a addition calculator', 'CSharp', 1, NULL, NULL, NULL, NULL, NOW(), NOW()),
('PasswordChecker', 'Implement a password checker', 'TypeScript', 1, NULL, NULL, NULL, NULL, NOW(), NOW()),
('SubtractionEmpty', 'Implement the a subtraction calculator', 'CSharp', 1, NULL, NULL, NULL, NULL, NOW(), NOW()),
('Addition', 'Implement the a addition calculator', 'CSharp', 1, 2, 3, 3, 0, NOW(), NOW()),
('Addition', 'Implement the a addition calculator', 'CSharp', 1, 3, 3, 2, 1, NOW(), NOW()),
('Addition', 'Implement the a addition calculator', 'CSharp', 1, 4, 3, 3, 0, NOW(), NOW()),
('Addition', 'Implement the a addition calculator', 'CSharp', 1, 5, 3, 3, 0, NOW(), NOW()),
('Addition', 'Implement the a addition calculator', 'CSharp', 1, 6, 0, 0, 3, NOW(), NOW()),
('PasswordChecker', 'Implement a password checker', 'TypeScript', 1, 2 , 3, 0, 0, NOW(), NOW()),
('PasswordChecker', 'Implement a password checker', 'TypeScript', 1, 3, 2, 1, 0, NOW(), NOW()),
('PasswordChecker', 'Implement a password checker', 'TypeScript', 1, 4, 3, 0, 0, NOW(), NOW());

-- ArrayOfSnippets
INSERT INTO ArrayOfSnippets (ExerciseId) VALUES
(1),
(2),
(3),
(4),
(5),
(6),
(7),
(8),
(9),
(10),
(11);

-- Snippets
INSERT INTO Snippet (Code, ReadonlySection, FileName, ArrayOfSnippetsId) VALUES
-- Snippets für Übung 1 (Addition)
('public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){', TRUE, 'Program.cs', 1),
('throw new NotImplementedException();', FALSE, 'Program.cs', 1),
('}}', TRUE, 'Program.cs', 1),

-- Snippets für Übung 2 (PasswordChecker)
('export function CheckPassword(password: string): boolean{', TRUE, 'passwordChecker.ts', 2),
('throw new Error(''Method not implemented.'');', FALSE, 'passwordChecker.ts', 2),
('}', TRUE, 'passwordChecker.ts', 2),

-- Snippets für Übung 3 (SubtractionEmpty)
('public class Program { static void Main(string[] args) {} public static int Subtract(int a, int b){', TRUE, 'Program.cs', 3),
('throw new NotImplementedException();', FALSE, 'Program.cs', 3),
('}}', TRUE, 'Program.cs', 3),

-- Snippets für Übung 4 (Addition)
('public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){', TRUE, 'Program.cs', 4),
('throw new NotImplementedException();', FALSE, 'Program.cs', 4),
('}}', TRUE, 'Program.cs', 4),

-- Snippets für Übung 5 (Addition)
('public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){', TRUE, 'Program.cs', 5),
('throw new NotImplementedException();', FALSE, 'Program.cs', 5),
('}}', TRUE, 'Program.cs', 5),

-- Snippets für Übung 6 (Addition)
('public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){', TRUE, 'Program.cs', 6),
('throw new NotImplementedException();', FALSE, 'Program.cs', 6),
('}}', TRUE, 'Program.cs', 6),

-- Snippets für Übung 7 (Addition)
('public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){', TRUE, 'Program.cs', 7),
('throw new NotImplementedException();', FALSE, 'Program.cs', 7),
('}}', TRUE, 'Program.cs', 7),

-- Snippets für Übung 8 (Addition)
('public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){', TRUE, 'Program.cs', 8),
('throw new NotImplementedException();', FALSE, 'Program.cs', 8),
('}}', TRUE, 'Program.cs', 8),

-- Snippets für Übung 9 (PasswordChecker)
('export function CheckPassword(password: string): boolean{', TRUE, 'passwordChecker.ts', 9),
('throw new Error(''Method not implemented.'');', FALSE, 'passwordChecker.ts', 9),
('}', TRUE, 'passwordChecker.ts', 9),

-- Snippets für Übung 10 (PasswordChecker)
('export function CheckPassword(password: string): boolean{', TRUE, 'passwordChecker.ts', 10),
('throw new Error(''Method not implemented.'');', FALSE, 'passwordChecker.ts', 10),
('}', TRUE, 'passwordChecker.ts', 10),

-- Snippets für Übung 11 (PasswordChecker)
('export function CheckPassword(password: string): boolean{', TRUE, 'passwordChecker.ts', 11),
('throw new Error(''Method not implemented.'');', FALSE, 'passwordChecker.ts', 11),
('}', TRUE, 'passwordChecker.ts', 11);

-- Assignments
INSERT INTO Assignments (Name, DateDue, ExerciseId, TeacherId) VALUES
('Assignment Addition', '2024-11-18 00:00:00', 1, 1),
('Assignment PasswordChecker', '2024-12-01 00:00:00', 2, 1);

-- AssignmentUsers
INSERT INTO AssignmentUser  (AssignmentId, StudentId) VALUES
(1, 2),  -- Assignment Addition for Student 2
(1, 3),  -- Assignment Addition for Student 3
(1, 4),  -- Assignment Addition for Student 4
(1, 5),  -- Assignment Addition for Student 5
(1, 6),  -- Assignment Addition for Student 6
(2, 2),  -- Assignment PasswordChecker for Student 2
(2, 3),  -- Assignment PasswordChecker for Student 3
(2, 4);  -- Assignment PasswordChecker for Student 4
