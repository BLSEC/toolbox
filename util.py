#!/usr/bin/env python
from argparse import ArgumentParser, ArgumentError


def parse_args():
    parser = ArgumentParser()
    try:
        parser.add_argument("-i", "--ip",
            type=str,
            required=True,
            help="IP address to scan.")
        parser.add_argument("-p", "--port",
            type=str,
            required=True,
            help="Specific port to scan on the IP address.")
    except ArgumentError as err:
        raise err
    else:
        return parser.parse_args()
