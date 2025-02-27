#!/usr/bin/env python
import subprocess, smtplib, re, os
from plyer import notification

def send_mail(email, passwd, message):
    server = smtplib.SMTP('smtp.gmail.com', 587)
    server.starttls()
    server.login(email, passwd)
    server.sendmail(email, email, message)
    server.quit()


command = 'netsh interface ipv4 show interfaces'

res = subprocess.check_output(command, shell=True).decode()
send_mail(config["email"]["ADDRESS"], passwd=config["email"]["PASS"], message=res)

networks = res.split('\n')

for n in networks:
    if n == '':
        continue

    n = n.strip('\r').strip('\n').split()
    if len(n) == 0:
        continue
    if '---' in n:
        continue
    print(f'[+] Network interface: {n[4]}')

# print(os.path.exists('test_356816.png'))
notification.notify(
    title='Here is the title',
    message='Here is the message',
    app_name='Here is the application name',
    # app_icon='icon-flexible.png'       I guess no icons?
)
