using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ITI
{
    public partial class ShowGradeForm : Form
    {
        private Dictionary<int, string> userAnswers;
        private ExamPageForm examPageForm;
        private DataGridView dataGridView;
        private int lastExamId;
        private int studentId;
        private int courseId;

        public ShowGradeForm(Dictionary<int, string> userAnswers, int lastExamId, int studentId, int courseId, ExamPageForm examPageForm)
        {
            InitializeComponent();
            // Add a label for the title
            Label titleLabel = new Label();
            titleLabel.Text = "Student Grade";
            titleLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            titleLabel.ForeColor = Color.DarkBlue;
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 20);
            this.Controls.Add(titleLabel);

            this.userAnswers = userAnswers;
            this.lastExamId = lastExamId;
            this.studentId = studentId;
            this.courseId = courseId;
            this.examPageForm = examPageForm;
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(20, 180); // Adjust the location to avoid overlapping with labels
            dataGridView.Size = new Size(400, 300);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "Question ID";
            dataGridView.Columns[1].Name = "User Answer";
            this.Load += ShowGradeForm_Load;
            this.Controls.Add(dataGridView);
            this.Text = "Exam Result";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 700); // Adjust the size as needed

            Label exa = new Label();
            exa.Text = "Student Answers :";
            exa.AutoSize = true;
            exa.Location = new Point(20, 160);
            this.Controls.Add(exa);

        }

        private void ShowGradeForm_Load(object sender, EventArgs e)
        {
            string studentName = FetchStudentNameFromDatabase(studentId);
            DisplayStudentName(studentName);

            string courseName = FetchCourseNameFromDatabase(courseId);
            DisplayCourseName(courseName);

            string instructorName = FetchInstructorNameFromDatabase(studentId, courseId);
            DisplayInstructorName(instructorName);

            PopulateDataGridView(); // Populate DataGridView with user answers
            SaveUserAnswersToDatabase(); // Save user answers to the database
            FetchTotalQuestionsCountFromDatabase();
            CalculateGradeAndDisplay(); // Calculate grade and display

            FetchAndDisplayModelAnswers(userAnswers); // Fetch and display model answers
        }
        private string FetchStudentNameFromDatabase(int studentId)
        {
            string studentName = "";
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT Std_FName FROM Student WHERE Std_Id = @studentId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@studentId", studentId);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        studentName = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Student not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while fetching student data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return studentName;
        }

        private void DisplayStudentName(string studentName)
        {
            Label studentNameLabel = new Label();
            studentNameLabel.AutoSize = true;
            studentNameLabel.Location = new Point(40, 80);
            studentNameLabel.Text = "Student Name: " + studentName;
            this.Controls.Add(studentNameLabel);
        }

        private string FetchCourseNameFromDatabase(int courseId)
        {
            string courseName = "";
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT Crs_Name FROM Course WHERE Crs_ID = @courseId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        courseName = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Course not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while fetching course data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return courseName;
        }

        private void DisplayCourseName(string courseName)
        {
            Label courseNameLabel = new Label();
            courseNameLabel.AutoSize = true;
            courseNameLabel.Location = new Point(40, 100); // Adjust the position as needed
            courseNameLabel.Text = "Course Name: " + courseName;
            this.Controls.Add(courseNameLabel);
        }
        private string FetchInstructorNameFromDatabase(int studentId, int courseId)
        {
            string instructorName = "";
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT I.Ins_Name FROM Instructor I JOIN Course C ON I.Ins_Id = C.Ins_Id JOIN Stud_Course SC ON SC.Crs_Id = C.Crs_Id WHERE SC.Std_Id = @studentId AND SC.Crs_Id = @courseId;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        instructorName = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Instructor not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while fetching instructor data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return instructorName;
        }


        private void DisplayInstructorName(string instructorName)
        {
            Label instructorNameLabel = new Label();
            instructorNameLabel.AutoSize = true;
            instructorNameLabel.Location = new Point(40, 120); // Adjust the position as needed
            instructorNameLabel.Text = "Instructor Name: " + instructorName;
            this.Controls.Add(instructorNameLabel);
        }
        private void FetchAndDisplayModelAnswers(Dictionary<int, string> userAnswers)
        {
            // Clear existing columns if any
            dataGridView.Columns.Clear();

            // Add columns for Question Number, User Answer, and Model Answer
            dataGridView.Columns.Add("Question Number", "Question Number");
            dataGridView.Columns.Add("User Answer", "User Answer");
            dataGridView.Columns.Add("Model Answer", "Model Answer");

            int questionNumber = 1; // Initialize the question number

            foreach (var kvp in userAnswers)
            {
                // Fetch model answer from the database
                string modelAnswer = FetchModelAnswerFromDatabase(kvp.Key);

                // Add row to the DataGridView with Question Number, User Answer, and Model Answer
                dataGridView.Rows.Add(questionNumber++, kvp.Value, modelAnswer);
            }

            // Adjust DataGridView properties to fit text and show full text
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


        private string FetchModelAnswerFromDatabase(int questionId)
        {
            string modelAnswer = "";
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT Model_ans_txt FROM Question WHERE Question_Id = @questionId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@questionId", questionId);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        modelAnswer = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Model answer not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while fetching model answer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return modelAnswer;
        }

        private void PopulateDataGridView()
        {
            int questionNumber = 1; // Initialize the question number

            foreach (var kvp in userAnswers)
            {
                string userAnswer = kvp.Value;

                // Add row to the DataGridView with sequential question numbers instead of actual question IDs
                dataGridView.Rows.Add(questionNumber++, userAnswer);
            }
        }


        private void SaveUserAnswersToDatabase()
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var kvp in userAnswers)
                    {
                        int questionId = kvp.Key;
                        string userAnswer = kvp.Value;

                        SqlCommand command = new SqlCommand("exam_answer", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@exam_ID", lastExamId);
                        command.Parameters.AddWithValue("@st_ID", studentId);
                        command.Parameters.AddWithValue("@q_ID", questionId);
                        command.Parameters.AddWithValue("@answer", userAnswer);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while saving user answers to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int FetchTotalQuestionsCountFromDatabase()
        {
            int totalQuestions = 0;
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT COUNT(*) FROM Exam_question WHERE Ex_Id = @lastExamId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@lastExamId", lastExamId);
                    connection.Open();
                    totalQuestions = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while fetching total questions count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalQuestions;
        }
        private void CalculateGradeAndDisplay()
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("exam_correction", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ExamId", lastExamId);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int grade = Convert.ToInt32(reader["Your Grade is"]);
                        DisplayGrade(grade);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while calculating and displaying the grade: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayGrade(int grade)
        {
            int totalQuestions = FetchTotalQuestionsCountFromDatabase();
            // Add a PictureBox for the image (passed/failed)
            PictureBox resultPictureBox = new PictureBox();
            resultPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            resultPictureBox.Size = new Size(300, 300); // Set the size to 300x300 pixels
            resultPictureBox.Location = new Point(550, 210); // Adjust the location as needed
            this.Controls.Add(resultPictureBox);

            string result = (grade >= (totalQuestions / 2)) ? "Passed, Congratulations" : "Failed, I advise to study harder before taking another try";

            Label gradeLabel = new Label();
            gradeLabel.AutoSize = true;
            gradeLabel.Location = new Point(200, dataGridView.Bottom + 60);
            this.Controls.Add(gradeLabel);

            if (grade >= (totalQuestions / 2))
            {
                resultPictureBox.Image = Image.FromFile("D:\\ITI\\BI ME\\20.GP\\Mine\\sty\\555.png");
            }
            else
            {
                resultPictureBox.Image = Image.FromFile("D:\\ITI\\BI ME\\20.GP\\Mine\\sty\\444.png");
            }

            gradeLabel.Text = $"Your grade is: {grade} out of {totalQuestions}. You have {result}.";


            // Add buttons for Home and Sign Out
            Button homeButton = new Button();
            homeButton.Text = "Home";
            homeButton.Location = new Point(350, dataGridView.Bottom + 100);
            homeButton.Click += (sender, e) => {
                // Navigate to the CourseSelectionForm
                CourseSelectionForm courseSelectionForm = new CourseSelectionForm(AuthenticationManager.studentId);
                courseSelectionForm.Show();
                this.Hide();
            };
            this.Controls.Add(homeButton);

            Button signOutButton = new Button();
            signOutButton.Text = "Sign Out";
            signOutButton.Location = new Point(450, dataGridView.Bottom + 100);
            signOutButton.Click += (sender, e) => {
                // Navigate to the LoginForm
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
            };
            this.Controls.Add(signOutButton);
        }

    }
}

