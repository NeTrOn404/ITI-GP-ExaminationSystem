using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ITI
{
    public static class AuthenticationManager
    {
        public static int studentId { get; private set; }

        public static bool AuthenticateUser(string email, string password)
        {
            string connectionString = "Server=DESKTOP-J15AEVB;Database=ITI-GP;Integrated Security=True;";
            string query = "SELECT Std_Id FROM Student WHERE Std_Email = @Email AND Std_Password = @Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        studentId = Convert.ToInt32(result);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception for debugging
                Console.WriteLine("SQL Exception: " + ex.Message);

                // Return false to indicate authentication failure
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine("Exception: " + ex.Message);

                // Return false to indicate authentication failure
                return false;
            }
        }
    }

    public partial class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm()
        {
            InitializeComponent();
            InitializeComponents();
            ApplyStyles();
        }

        private void InitializeComponents()
        {
            this.Text = "Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 600);

            Label lblWelcome = new Label();
            lblWelcome.Text = "Welcome to ITI Exam App";
            lblWelcome.Font = new Font("Arial", 16, FontStyle.Bold);
            lblWelcome.Location = new Point(50, 110); // Position the label above the Email label
            lblWelcome.AutoSize = true;
            this.Controls.Add(lblWelcome);

            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(50, 150 + 80); // Move the Email label down by 80 pixels
            lblEmail.AutoSize = true;
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Location = new Point(150, 150 + 80); // Move the Email textbox down by 80 pixels
            txtEmail.Size = new Size(300, 30);
            this.Controls.Add(txtEmail);

            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(50, 200 + 80); // Move the Password label down by 80 pixels
            lblPassword.AutoSize = true;
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(150, 200 + 80); // Move the Password textbox down by 80 pixels
            txtPassword.Size = new Size(300, 30);
            txtPassword.PasswordChar = '*';
            this.Controls.Add(txtPassword);

            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(250, 250 + 80); // Move the Login button down by 80 pixels
            btnLogin.Size = new Size(150, 40);
            btnLogin.Click += btnLogin_Click;
            this.Controls.Add(btnLogin);

            // Add PictureBox for the image
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile(@"D:\ITI\BI ME\20.GP\Mine\sty\11.png");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Size = new Size(400, 400);
            pictureBox.Location = new Point(this.Width - 450, 50);
            this.Controls.Add(pictureBox);
        }

        private void ApplyStyles()
        {
            Font titleFont = new Font("Arial", 24, FontStyle.Bold);
            Label titleLabel = new Label();
            titleLabel.Text = "Login";
            titleLabel.Font = titleFont;
            titleLabel.ForeColor = Color.DarkBlue;
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 20);
            this.Controls.Add(titleLabel);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            // Add your authentication logic here
            bool isAuthenticated = AuthenticationManager.AuthenticateUser(email, password);

            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Pass the StudentId to the ExamPageForm
                CourseSelectionForm courseSelectionForm = new CourseSelectionForm(AuthenticationManager.studentId);
                courseSelectionForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
