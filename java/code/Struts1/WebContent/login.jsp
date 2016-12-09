<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"%>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
<title>Insert title here</title>
</head>
<body>
<form id="f1" action="/Struts1/login.do" method="post">  
    userName<input type="text" name="name" size="30" /><br/>  
    pwd<input type="password" name="password" size="30" /><br/>  
    <input type="submit" value="Login" />  
</form>
<h3>---------------------------------------------------------------------------</h3>
<form id="f2" action="/Struts1/lazy.do" method="post" >
	name<input type="text" name="name" size="30" /><br/>  
    age<input type="text" name="age" size="30" /><br/>  
    <input type="submit" value="call" />  
</form>


</body>
</html>