using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ITI
{
    public partial class ExamPageForm : Form
    {
        private string selectedCourseName;
        private int courseId;
        private int lastExamId;
        private int studentId;
        private Dictionary<int, Tuple<string, List<string>>> questionChoices;
        private Dictionary<int, List<RadioButton>> questionRadioButtons;
        private Button submitButton;
        private Panel questionPanel;

        public ExamPageForm(string selectedCourseName, int studentId)
        {
            InitializeComponent();
            this.selectedCourseName = selectedCourseName;
            this.studentId = studentId;
            questionChoices = new Dictionary<int, Tuple<string, List<string>>>();
            questionRadioButtons = new Dictionary<int, List<RadioButton>>();
            GetCourseId();
            GenerateExam();
            LoadQuestionsAndChoices();
            DisplayQuestionsAndChoices();
            this.FormClosing += ExamPageForm_FormClosing;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 650); // Change the size to fit the maximized state
            this.Name = "ExamPageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exam Page";
            this.ResumeLayout(false);

            // Add PictureBox for the image
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("D:\\ITI\\BI ME\\20.GP\\Mine\\sty\\33.png"); // Specify the path to your image
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Adjust the image size mode as needed

            // Adjust the size of the picture box
            pictureBox.Size = new Size(150, 150); // Adjust the size of the picture box (150x150 pixels)

            int offsetX = 120; // Move it 20 pixels to the left
            int offsetY = 120; // Move it 20 pixels down
            int middleY = (this.Height - pictureBox.Height) / 2; // Calculate the middle Y coordinate
            pictureBox.Location = new Point(this.Width - pictureBox.Width - offsetX, middleY + offsetY); // Position the picture box

            this.Controls.Add(pictureBox);
        }

        private void GetCourseId()
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Crs_Id FROM Course WHERE Crs_Name = @CrsName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CrsName", selectedCourseName);
                    connection.Open();
                    courseId = (int)command.ExecuteScalar();
                }
            }
        }

        private void GenerateExam()
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            int generatedExamId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("generate_exam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Crs_ID", courseId);
                    command.Parameters.AddWithValue("@num_mcq", 7);
                    command.Parameters.AddWithValue("@num_tf", 3);

                    SqlParameter outputParameter = new SqlParameter("@Exam_ID", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                    connection.Open();
                    command.ExecuteNonQuery();

                    if (outputParameter.Value != DBNull.Value && outputParameter.Value != null)
                    {
                        generatedExamId = Convert.ToInt32(outputParameter.Value);
                    }
                    else
                    {
                        MessageBox.Show("No exam ID returned from the stored procedure.");
                    }
                }
            }

            lastExamId = generatedExamId;
        }

        private void LoadQuestionsAndChoices()
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Q.Question_Id, Q.Question_Text, C.Choice_Text, C.Is_correct
            FROM Question Q
            JOIN Choice C ON Q.Question_Id = C.Question_Id
            JOIN Exam_question EQ ON Q.Question_Id = EQ.Question_Id
            WHERE EQ.Ex_Id = @ExamId
            ORDER BY EQ.Question_Id, C.Choice_Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExamId", lastExamId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    int prevQuestionId = 0;
                    List<string> choices = new List<string>();
                    string questionText = string.Empty;

                    while (reader.Read())
                    {
                        int questionId = Convert.ToInt32(reader["Question_Id"]);
                        questionText = reader["Question_Text"].ToString();
                        string choiceText = reader["Choice_Text"].ToString();

                        if (questionId != prevQuestionId)
                        {
                            if (prevQuestionId != 0)
                            {
                                questionChoices.Add(prevQuestionId, new Tuple<string, List<string>>(questionText, choices));
                                choices = new List<string>();
                            }

                            prevQuestionId = questionId;
                        }

                        choices.Add(choiceText);
                    }

                    if (prevQuestionId != 0)
                    {
                        questionChoices.Add(prevQuestionId, new Tuple<string, List<string>>(questionText, choices));
                    }
                }
            }
        }

        private void DisplayQuestionsAndChoices()
        {
            questionPanel = new Panel
            {
                AutoScroll = true,
                Location = new Point(20, 20),
                Size = new Size(800, 600)
            };
            Controls.Add(questionPanel);

            int yOffset = 20;

            foreach (var (questionId, questionData) in questionChoices)
            {
                var choices = questionData.Item2;
                var questionGroup = new GroupBox
                {
                    Text = $"{questionData.Item1}",
                    AutoSize = true,
                    Location = new Point(0, yOffset),
                    Width = 700
                };
                questionPanel.Controls.Add(questionGroup);

                var radioButtons = new List<RadioButton>();
                int radioYOffset = 20;

                foreach (var choice in choices)
                {
                    var radioButton = new RadioButton
                    {
                        Text = choice,
                        AutoSize = true,
                        Location = new Point(20, radioYOffset)
                    };
                    questionGroup.Controls.Add(radioButton);
                    radioButtons.Add(radioButton);
                    radioYOffset += 20;
                }
                yOffset += questionGroup.Height + 10;
                questionRadioButtons.Add(questionId, radioButtons);
            }

            submitButton = new Button
            {
                Text = "Submit",
                Location = new Point(20, yOffset),
            };
            submitButton.Click += SubmitButton_Click;
            questionPanel.Controls.Add(submitButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
{
    Dictionary<int, string> userAnswers = new Dictionary<int, string>();

    foreach (var kvp in questionRadioButtons)
    {
        int questionId = kvp.Key;
        List<RadioButton> radioButtons = kvp.Value;

        RadioButton selectedRadioButton = radioButtons.FirstOrDefault(rb => rb.Checked);
        
        // If no radio button is checked, add an empty string as the user answer
        string selectedChoice = selectedRadioButton != null ? selectedRadioButton.Text : "";

        userAnswers.Add(questionId, selectedChoice);
    }

    ShowGradeForm showGradeForm = new ShowGradeForm(userAnswers, this.lastExamId, this.studentId, this.courseId, this);
    showGradeForm.Show();
    this.Close();
}


        private void ExamPageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                this.Close();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ExamPageForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "ExamPageForm";
            this.ResumeLayout(false);
        }
    }
}
