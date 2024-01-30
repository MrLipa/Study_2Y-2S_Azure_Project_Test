import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

class EmailSender:
    def __init__(self, from_email, from_password, smtp_server, smtp_port):
        self.from_email = from_email
        self.from_password = from_password
        self.smtp_server = smtp_server
        self.smtp_port = smtp_port

    def send_email(self, to_email, subject, message):
        try:
            msg = MIMEMultipart()
            msg['Subject'] = subject
            msg['From'] = self.from_email
            msg['To'] = to_email

            msg.attach(MIMEText(message, 'plain'))

            with smtplib.SMTP(self.smtp_server, self.smtp_port) as server:
                server.starttls()
                server.login(self.from_email, self.from_password)
                server.sendmail(self.from_email, to_email, msg.as_string())
            return True
        except Exception as e:
            print(f"An error occurred: {e}")
            return False