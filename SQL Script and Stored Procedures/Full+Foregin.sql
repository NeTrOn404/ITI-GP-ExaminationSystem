-- Create Department Table
CREATE TABLE Department (
    Dept_Id INT PRIMARY KEY,
    Dept_Name VARCHAR(100),
    Dept_Location VARCHAR(100),
    Dept_Budget DECIMAL(15, 2),
    Dept_NoEmp INT,
    Dept_Email VARCHAR(100),
    Ins_Id_mgr INT,
    Dept_Mgr_HireDate DATE
);

-- Create Instructor Table
CREATE TABLE Instructor (
    Ins_Id INT PRIMARY KEY,
    Ins_Name VARCHAR(100),
    Ins_Gender VARCHAR(10),
    Ins_Email VARCHAR(100),
    Ins_Degree VARCHAR(100),
    Ins_Company VARCHAR(100),
    Ins_Position VARCHAR(100),
    Ins_Salary DECIMAL(10, 2),
    Ins_Password VARCHAR(100),
    Ins_Id_Mgr INT,
    Dept_Id INT
);

-- Create Course Table
CREATE TABLE Course (
    Crs_Id INT PRIMARY KEY,
    Crs_Name VARCHAR(100),
    Crs_Duration INT,
    Crs_NoHours INT,
    Crs_Fees DECIMAL(10, 2),
    Ins_Id INT
);

-- Create Student Table
CREATE TABLE Student (
    Std_Id INT PRIMARY KEY,
    Std_FName VARCHAR(50),
    Std_Age INT,
    Std_Gender VARCHAR(10),
    Std_Address VARCHAR(100),
    Std_City VARCHAR(50),
    Std_Faculty VARCHAR(50),
    Std_Email VARCHAR(100),
    Std_Password VARCHAR(100),
    Kpi_Id INT,
    Freelance_Status TINYINT,
    Freelance_Paid INT,
    Cert_Status TINYINT,
    Dept_Id INT
);

-- Create Job_Offer Table
CREATE TABLE Job_Offer (
    Job_Id INT PRIMARY KEY,
    Job_Hiring_Status TINYINT,
    Job_Title VARCHAR(100),
    Job_Salary DECIMAL(10, 2),
    Std_Id INT
);

-- Create Exam Table
CREATE TABLE Exam (
    Ex_Id INT IDENTITY(1,1) PRIMARY KEY,
    Ex_Name VARCHAR(100),
    Ex_NoQuestions INT,
    Ex_Duration INT,
    Ex_Date DATE,
    Ex_grade TINYINT,
    Crs_Id INT
);

-- Create Choice Table
CREATE TABLE Choice (
    Choice_id INT PRIMARY KEY,
    Choice_Text VARCHAR(200),
    Is_correct BIT,
    Question_Id INT
);

-- Create Question Table
CREATE TABLE Question (
    Question_Id INT PRIMARY KEY,
    Question_Text VARCHAR(500),
    Quest_type TINYINT,
    Quest_mark DECIMAL(5, 2),
    Model_ans_txt VARCHAR(500),
	Crs_Id INT
);

-- Create Student_Exam_Quest Table
CREATE TABLE Student_Exam_Quest (
    Std_Id INT,
    Question_Id INT,
    Ex_Id INT,
    Std_Qs_Grade DECIMAL(5, 2),
    Std_Ans VARCHAR(500),
    Ans_Date DATE
);

-- Create Student_Course Table
CREATE TABLE Stud_Course (
    Std_Id INT,
    Crs_Id INT,
    PRIMARY KEY (Std_Id, Crs_Id)
);

-- Create Student_Phone Table
CREATE TABLE Std_Phone (
    Std_Id INT,
    Std_Phone VARCHAR(20),
    PRIMARY KEY (Std_Id, Std_Phone)
);

-- Create Student_Course_Evaluate Table
CREATE TABLE Std_Crs_Evaluate (
    Std_Id INT,
    Crs_Id INT,
    Inst_ClassTime VARCHAR(50),
    Crs_well_organised VARCHAR(50),
    Inst_GiveClearEx VARCHAR(50),
    Inst_Response_Qus VARCHAR(50),
    Crs_Material_helpful VARCHAR(50),
    Crs_Content VARCHAR(50),
    PRIMARY KEY (Std_Id, Crs_Id)
);
CREATE TABLE [dbo].[Exam_question](
    [Ex_Id] INT NOT NULL,
    [Question_Id] INT NOT NULL,
    CONSTRAINT PK_Exam_question PRIMARY KEY (Ex_Id, Question_Id),
);




-- Add Foreign Key Constraints

-- Add Foreign Key to Student table
ALTER TABLE Student
ADD CONSTRAINT FK_Student_Dept FOREIGN KEY (Dept_Id) REFERENCES Department(Dept_Id);

ALTER TABLE Course
ADD CONSTRAINT FK_Course_Instructor FOREIGN KEY (Ins_Id) REFERENCES Instructor(Ins_Id);

-- Add Foreign Key to Student_Course table
ALTER TABLE Stud_Course
ADD CONSTRAINT FK_Stud_Course_Student FOREIGN KEY (Std_Id) REFERENCES Student(Std_Id);

ALTER TABLE Stud_Course
ADD CONSTRAINT FK_Stud_Course_Course FOREIGN KEY (Crs_Id) REFERENCES Course(Crs_Id);

-- Add Foreign Key to Std_Phone table
ALTER TABLE Std_Phone
ADD CONSTRAINT FK_Std_Phone_Student FOREIGN KEY (Std_Id) REFERENCES Student(Std_Id);

-- Add Foreign Key to Std_Crs_Evaluate table
ALTER TABLE Std_Crs_Evaluate
ADD CONSTRAINT FK_Std_Crs_Evaluate_Student FOREIGN KEY (Std_Id) REFERENCES Student(Std_Id);

ALTER TABLE Std_Crs_Evaluate
ADD CONSTRAINT FK_Std_Crs_Evaluate_Course FOREIGN KEY (Crs_Id) REFERENCES Course(Crs_Id);

-- Add Foreign Key to Job_Offer table
ALTER TABLE Job_Offer
ADD CONSTRAINT FK_Job_Offer_Student FOREIGN KEY (Std_Id) REFERENCES Student(Std_Id);

-- Add Foreign Key to Department table
ALTER TABLE Department
ADD CONSTRAINT FK_Department_Instructor FOREIGN KEY (Ins_Id_mgr) REFERENCES Instructor(Ins_Id);

-- Add Foreign Key to Exam table
ALTER TABLE Exam
ADD CONSTRAINT FK_Exam_Course FOREIGN KEY (Crs_Id) REFERENCES Course(Crs_Id);

-- Add Foreign Key to Choice table
ALTER TABLE Choice
ADD CONSTRAINT FK_Choice_Question FOREIGN KEY (Question_Id) REFERENCES Question(Question_Id);

-- Add Foreign Key to Student_Exam_Quest table
ALTER TABLE Student_Exam_Quest
ADD CONSTRAINT FK_Student_Exam_Quest_Student FOREIGN KEY (Std_Id) REFERENCES Student(Std_Id);

ALTER TABLE Student_Exam_Quest
ADD CONSTRAINT FK_Student_Exam_Quest_Question FOREIGN KEY (Question_Id) REFERENCES Question(Question_Id);

ALTER TABLE Student_Exam_Quest
ADD CONSTRAINT FK_Student_Exam_Quest_Exam FOREIGN KEY (Ex_Id) REFERENCES Exam(Ex_Id);

-- Add Foreign Key to Instructor table
ALTER TABLE Instructor
ADD CONSTRAINT FK_Instructor_Instructor FOREIGN KEY (Ins_Id_Mgr) REFERENCES Instructor(Ins_Id);

ALTER TABLE Instructor
ADD CONSTRAINT FK_Instructor_Department FOREIGN KEY (Dept_Id) REFERENCES Department(Dept_Id);

ALTER TABLE dbo.Exam_question
ADD CONSTRAINT FK_Exam_questionq_ FOREIGN KEY (Quetion_Id) REFERENCES dbo.Question(Question_Id);

ALTER TABLE dbo.Exam_question
ADD CONSTRAINT FK_Exam_question_e FOREIGN KEY (Ex_Id) REFERENCES dbo.Exam(Ex_Id);






-- Drop foreign key constraints
ALTER TABLE Instructor DROP CONSTRAINT FK_Instructor_Department;
ALTER TABLE Department DROP CONSTRAINT FK_Department_Instructor;

-- Generate data for affected tables (Instructor and Department)

-- Recreate foreign key constraints
ALTER TABLE Instructor ADD CONSTRAINT FK_Instructor_Department FOREIGN KEY (Dept_Id) REFERENCES Department(Dept_Id);
ALTER TABLE Department ADD CONSTRAINT FK_Department_Instructor FOREIGN KEY (Ins_Id_mgr) REFERENCES Instructor(Ins_Id);



INSERT INTO Student (Std_Id, Std_FName, Std_Age, Std_Gender, Std_Address, Std_City, Std_Faculty, Std_Email, Std_Password, Dept_Id)
VALUES 
    (1, 'John Doe', 20, 'Male', '123 Main St', 'Anytown', 'Computer Science', 'john@example.com', 'password123', 1),
    (2, 'Jane Smith', 22, 'Female', '456 Elm St', 'Othertown', 'Engineering', 'jane@example.com', 'secret456', 2);


INSERT INTO Department (Dept_Id, Dept_Name, Dept_Location, Dept_Budget, Dept_NoEmp, Dept_Email, Ins_Id_mgr)
VALUES 
    (1, 'Computer Science Department', 'Main Campus', 1000000, 50, 'csdept@example.com', 1),
    (2, 'Engineering Department', 'Main Campus', 1200000, 60, 'engdept@example.com', 2);

INSERT INTO Instructor (Ins_Id, Ins_Name, Ins_Gender, Ins_Email, Ins_Degree, Ins_Company, Ins_Position, Ins_Salary, Ins_Password, Ins_Id_Mgr, Dept_Id)
VALUES 
    (1, 'Professor Smith', 'Male', 'profsmith@example.com', 'Ph.D. Computer Science', 'University', 'Professor', 80000, 'password1', NULL, 1),
    (2, 'Professor Johnson', 'Female', 'profjohnson@example.com', 'Ph.D. Engineering', 'University', 'Professor', 85000, 'password2', NULL, 2);
INSERT INTO Course (Crs_Id, Crs_Name, Crs_Duration, Crs_NoHours, Crs_Fees, Ins_Id)
VALUES 
    (1, 'Introduction to Programming', '90', 90, 1000, 1),
    (2, 'Mechanical Engineering Fundamentals', '120', 120, 1200, 2);
INSERT INTO Choice (Choice_id, Choice_Text, Is_correct, Question_Id)
VALUES 
    (1, 'Paris', 1, 1),
    (2, 'London', 0, 1),
    (3, 'Berlin', 0, 1),
    (4, '4', 1, 2),
    (5, '5', 0, 2),
    (6, '6', 0, 2),
    (7, 'Yes', 1, 3),
    (8, 'No', 0, 3),
    (9, 'Yes', 1, 4),
    (10, 'No', 0, 4),
    (11, 'Yen', 1, 5),
    (12, 'Dollar', 0, 5),
    (13, 'Tokyo', 1, 6),
    (14, 'Osaka', 0, 6),
    (15, 'Mount Everest', 1, 7),
    (16, 'K2', 0, 7),
    (17, 'Yes', 1, 8),
    (18, 'No', 0, 8),
    (19, 'Ruble', 1, 9),
    (20, 'Euro', 0, 9),
    (21, 'No', 1, 10),
    (22, 'Yes', 0, 10);
INSERT INTO Choice (Choice_id, Choice_Text, Is_correct, Question_Id)
VALUES 
    (23, 'Newton', 1, 11),
    (24, 'Einstein', 0, 11),
    (25, 'Galileo', 0, 11),
    (26, 'Pythagoras', 1, 12),
    (27, 'Euclid', 0, 12),
    (28, 'Archimedes', 0, 12),
    (29, 'Great Wall of China', 1, 13),
    (30, 'Taj Mahal', 0, 13),
    (31, 'Eiffel Tower', 0, 13),
    (32, 'Mona Lisa', 1, 14),
    (33, 'The Last Supper', 0, 14),
    (34, 'Starry Night', 0, 14),
    (35, 'English', 1, 15),
    (36, 'Mandarin', 0, 15),
    (37, 'French', 0, 15),
    (38, 'Australia', 1, 16),
    (39, 'Canada', 0, 16),
    (40, 'Brazil', 0, 16),
    (41, 'Mercury', 1, 17),
    (42, 'Saturn', 0, 17),
    (43, 'Neptune', 0, 17),
    (44, 'Football', 1, 18),
    (45, 'Cricket', 0, 18),
    (46, 'Basketball', 0, 18),
    (47, 'Soccer', 1, 19),
    (48, 'Baseball', 0, 19),
    (49, 'Tennis', 0, 19),
    (50, 'Great Barrier Reef', 1, 20),
    (51, 'Galápagos Islands', 0, 20),
    (52, 'Lake Baikal', 0, 20);

INSERT INTO Choice (Choice_id, Choice_Text, Is_correct, Question_Id)
VALUES 
    (53, 'Newton', 1, 21),
    (54, 'Einstein', 0, 21),
    (55, 'Galileo', 0, 21),
    (56, 'Pythagoras', 1, 22),
    (57, 'Euclid', 0, 22),
    (58, 'Archimedes', 0, 22),
    (59, 'Great Wall of China', 1, 23),
    (60, 'Taj Mahal', 0, 23),
    (61, 'Eiffel Tower', 0, 23),
    (62, 'Mona Lisa', 1, 24),
    (63, 'The Last Supper', 0, 24),
    (64, 'Starry Night', 0, 24),
    (65, 'English', 1, 25),
    (66, 'Mandarin', 0, 25),
    (67, 'French', 0, 25),
    (68, 'Australia', 1, 26),
    (69, 'Canada', 0, 26),
    (70, 'Brazil', 0, 26),
    (71, 'Mercury', 1, 27),
    (72, 'Saturn', 0, 27),
    (73, 'Neptune', 0, 27),
    (74, 'Football', 1, 28),
    (75, 'Cricket', 0, 28),
    (76, 'Basketball', 0, 28),
    (77, 'Soccer', 1, 29),
    (78, 'Baseball', 0, 29),
    (79, 'Tennis', 0, 29),
    (80, 'Great Barrier Reef', 1, 30),
    (81, 'Galápagos Islands', 0, 30),
    (82, 'Lake Baikal', 0, 30);

-- Insert Choices for Type 1 True/False Questions
INSERT INTO Choice (Choice_id, Choice_Text, Is_correct, Question_Id)
VALUES 
    (83, 'True', 0, 31),
    (84, 'False', 1, 31),
    (85, 'True', 1, 32),
    (86, 'False', 0, 32),
    (87, 'True', 0, 33),
    (88, 'False', 1, 33),
    (89, 'True', 0, 34),
    (90, 'False', 1, 34),
    (91, 'True', 1, 35),
    (92, 'False', 0, 35);


INSERT INTO Question (Question_Id, Question_Text, Quest_type, Quest_mark, Model_ans_txt, Crs_Id)
VALUES 
    (1, 'What is the capital of France?', 0, 10, 'Paris', 1),
    (2, 'What is 2 + 2?', 1, 5, '4', 1),
    (3, 'Is the sky blue?', 0, 5, 'Yes', 1),
    (4, 'Is the sun made of gas?', 1, 10, 'Yes', 1),
    (5, 'What is the currency of Japan?', 0, 8, 'Yen', 1),
    (6, 'What is the capital of Japan?', 0, 10, 'Tokyo', 1),
    (7, 'What is the tallest mountain in the world?', 1, 10, 'Mount Everest', 1),
    (8, 'Is water composed of hydrogen and oxygen?', 1, 8, 'Yes', 1),
    (9, 'What is the currency of Russia?', 0, 7, 'Ruble', 1),
    (10, 'Is the earth flat?', 0, 5, 'No', 1);

INSERT INTO Question (Question_Id, Question_Text, Quest_Type, Quest_Mark, Model_ans_txt, Crs_Id)
VALUES 
    (11, 'Who is credited with discovering the law of gravity?', 0, 10, 'Newton', 1),
    (12, 'Who formulated the Pythagorean theorem?', 0, 10, 'Pythagoras', 1),
    (13, 'Which landmark is located in China?', 0, 10, 'Great Wall of China', 1),
    (14, 'Who painted the Mona Lisa?', 0, 10, 'Leonardo da Vinci', 1),
    (15, 'Which language is spoken in Australia?', 0, 8, 'English', 1),
    (16, 'Which country is known for the Amazon Rainforest?', 0, 10, 'Brazil', 1),
    (17, 'Which planet is closest to the sun?', 0, 10, 'Mercury', 1),
    (18, 'Which sport is known as "The Beautiful Game"?', 0, 8, 'Football', 1),
    (19, 'Which natural wonder is off the coast of Australia?', 0, 10, 'Great Barrier Reef', 1),
    (20, 'Which is the largest reef system in the world?', 0, 10, 'Great Barrier Reef', 1);


INSERT INTO Question (Question_Id, Question_Text, Quest_Type, Quest_Mark, Model_ans_txt, Crs_Id)
VALUES 
    (21, 'Who is credited with discovering the law of gravity?', 0, 10, 'Newton', 2),
    (22, 'Who formulated the Pythagorean theorem?', 0, 10, 'Pythagoras', 2),
    (23, 'Which landmark is located in China?', 0, 10, 'Great Wall of China', 2),
    (24, 'Who painted the Mona Lisa?', 0, 10, 'Leonardo da Vinci', 2),
    (25, 'Which language is spoken in Australia?', 0, 8, 'English', 2),
    (26, 'Which country is known for the Amazon Rainforest?', 0, 10, 'Brazil', 2),
    (27, 'Which planet is closest to the sun?', 0, 10, 'Mercury', 2),
    (28, 'Which sport is known as "The Beautiful Game"?', 0, 8, 'Football', 2),
    (29, 'Which natural wonder is off the coast of Australia?', 0, 10, 'Great Barrier Reef', 2),
    (30, 'Which is the largest reef system in the world?', 0, 10, 'Great Barrier Reef', 2);

		-- Insert Type 1 True/False Questions for Course 1
INSERT INTO Question (Question_Id, Question_Text, Quest_type, Quest_mark, Model_ans_txt, Crs_Id)
VALUES 
    (31, 'The sun rises in the west.', 1, 5, 'False', 1),
    (32, 'Water boils at 100 degrees Celsius.', 1, 5, 'True', 1),
    (33, 'The Statue of Liberty is located in Paris.', 1, 5, 'False', 1),
    (34, 'Albert Einstein discovered gravity.', 1, 5, 'False', 1),
    (35, 'Birds can fly.', 1, 5, 'True', 1);



