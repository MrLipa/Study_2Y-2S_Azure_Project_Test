import logging
import azure.functions as func
from EmailSender import EmailSender
import json
import os

app = func.FunctionApp()

@app.event_grid_trigger(arg_name="azeventgrid")
def EventGridTrigger(azeventgrid: func.EventGridEvent):
    try:
        data = azeventgrid.get_json()
    except ValueError:
        logging.info('Error processing event grid data.')
        return

    email = data.get('email')
    subject = data.get('subject')
    message = data.get('message')

    logging.info(f'{email} {subject} {message}')

    email_username = os.environ.get('EMAIL_USERNAME', None)
    email_password = os.environ.get('EMAIL_PASSWORD', None)

    logging.info(f'Send by {email_username}')

    email_sender = EmailSender(email_username, 
                               email_password, 
                               "smtp.ethereal.email", 
                               587)
    
    if email and subject and message:
        success = email_sender.send_email(email, subject, message)
        if success:
            logging.info('Email sent successfully.')
        else:
            logging.info('Failed to send email.')
    else:
        logging.info('Email, subject, or message is missing.')
