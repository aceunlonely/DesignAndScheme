package com.jumpig.struts.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.struts.action.Action;
import org.apache.struts.action.ActionForm;
import org.apache.struts.action.ActionForward;
import org.apache.struts.action.ActionMapping;

import com.jumpig.struts.form.UserForm;

public class LoginAction extends Action {
	public ActionForward execute(ActionMapping mapping, ActionForm form,HttpServletRequest request, HttpServletResponse response) {  
        UserForm userForm = (UserForm)form;  
        String name = userForm.getName();  
        String password = userForm.getPassword();  
        if (name.equals("admin")&& password.equals("123456")) {  
            return mapping.findForward("success");  
        } else {  
            request.setAttribute("message", "用户名或密码错误！");  
            return mapping.findForward("fail");  
        }  
    }  
}
