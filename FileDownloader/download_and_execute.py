#!/usr/bin/env python
import requests, subprocess, os, tempfile, time

def download(url):
    resp = requests.get(url)
    filename = url.split('/')[-1]
    with open(filename, 'wb') as output:
        output.write(resp.content)


temp_dir = tempfile.gettempdir()
os.chdir(temp_dir)
print(f'[*] Temp dir: {temp_dir}')
download('https://i.ytimg.com/vi/PB5FosTwM8s/maxresdefault.jpg')
print('[-] Deleting after a nap...')
