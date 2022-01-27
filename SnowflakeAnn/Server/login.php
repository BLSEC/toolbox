<?php 

include 'conn.php';

if($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["username"]) && isset($_POST["password"]))
{
    $loginQuery = $mysql->prepare("select * from users where username=? and password=?");
    $loginQuery->bind_param("ss", $_POST["username"], MD5($_POST["password"]));
    $loginQuery->execute();
    $loginQuery->store_result();

    if($loginQuery->num_rows > 0)
    {
        session_start();
        $_SESSION["username"] = $_POST["username"];
        header("Location: /index.php");

    }
    else {
        echo "Wrong username and/or password";
    }
}
?>

<html>
    <form action="" method="POST">
        <input type="text" name="username" placeholder="username"/>
        <input type="text" name="password" placeholder="password"/>
        <input type="submit" name="submit" value="log-in"/>
    </form>
</html>