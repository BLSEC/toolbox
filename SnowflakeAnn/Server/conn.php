<?php

$mysql = mysqli_connect('localhost', 'root', 'ubuntu', 'ControlPanel'); // TODO: Re-set MySql up later.... name should be "targets" instead of "victims"

if (mysqli_connect_errno())
{
    echo "Failed to connect to database:".mysqli_connect_errno();
    exit();
}