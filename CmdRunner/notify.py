#!/usr/bin/env python
import subprocess, smtplib, re, os, time
from plyer import notification

if __name__ == '__main__':
    try:
        notification.notify(
            title='Test title',
            message='Test message',
            app_name='Test',
        )
    except Exception as error:
        print('!] Blah blah blah generic exception')

    print('[-] Sleeping...')
    time.sleep(10)
