using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ITI
{
    public partial class CourseSelectionForm : Form
    {
        private int studentId;
        private PictureBox pictureBox1;
        private ComboBox comboBox1;
        private Button button1;

        public CourseSelectionForm(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            InitializeComponents();
            ApplyStyles();
            LoadCourses(); // Load courses when the form initializes
        }

        private void InitializeComponents()
        {
            this.Text = "Course Selection";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(1000, 600);

            // Increase the font size of the label
            Label labelSelectCourse = new Label();
            labelSelectCourse.Text = "Select the course of the exam";
            labelSelectCourse.Font = new Font(labelSelectCourse.Font.FontFamily, 14, FontStyle.Regular);
            labelSelectCourse.AutoSize = true;
            labelSelectCourse.Location = new System.Drawing.Point(100, 250); // Moved down
            this.Controls.Add(labelSelectCourse);

            comboBox1 = new ComboBox();
            comboBox1.Location = new System.Drawing.Point(100, 300); // Moved down
            comboBox1.Size = new System.Drawing.Size(300, 30);
            this.Controls.Add(comboBox1);

            button1 = new Button();
            button1.Text = "Start Exam";
            button1.Location = new System.Drawing.Point(150, 350); // Moved down
            button1.Size = new System.Drawing.Size(150, 40);
            button1.Click += button1_Click;
            this.Controls.Add(button1);

            pictureBox1 = new PictureBox();
            pictureBox1.Image = Image.FromFile("D:\\ITI\\BI ME\\20.GP\\Mine\\sty\\22.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            // Increase the size of the picture by 1.75
            pictureBox1.Size = new System.Drawing.Size((int)(300 * 1.75), (int)(300 * 1.75));
            // Move the picture more to the down-right
            pictureBox1.Location = new Point(this.Width - pictureBox1.Width - 50, this.Height - pictureBox1.Height - 50);
            this.Controls.Add(pictureBox1);
        }

        private void ApplyStyles()
        {
            Font titleFont = new Font("Arial", 24, FontStyle.Bold);
            Label titleLabel = new Label();
            titleLabel.Text = "Course Selection";
            titleLabel.Font = titleFont;
            titleLabel.ForeColor = System.Drawing.Color.DarkBlue;
            titleLabel.AutoSize = true;
            titleLabel.Location = new System.Drawing.Point(20, 20);
            this.Controls.Add(titleLabel);
        }

        private void LoadCourses()
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT Crs_Name FROM Course";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["Crs_Name"].ToString());
                }

                reader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if a course is selected
            if (comboBox1.SelectedItem != null)
            {
                // Get the selected course name
                string selectedCourseName = comboBox1.SelectedItem.ToString();

                // Navigate to the ExamPageForm and pass the selected course name
                ExamPageForm examPageForm = new ExamPageForm(selectedCourseName, AuthenticationManager.studentId);
                examPageForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please select a course.");
            }
        }
    }
}
