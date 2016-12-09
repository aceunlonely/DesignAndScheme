package com.jumpig.struts.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.struts.action.Action;
import org.apache.struts.action.ActionForm;
import org.apache.struts.action.ActionForward;
import org.apache.struts.action.ActionMapping;
import org.apache.struts.validator.LazyValidatorForm;

public class LazyAction extends Action {
	public ActionForward execute(ActionMapping mapping, ActionForm form,HttpServletRequest request, HttpServletResponse response) {  
		LazyValidatorForm lzForm = (LazyValidatorForm)form;  
        String name = lzForm.get("name").toString();
        String age = lzForm.get("age").toString(); 
        if (Integer.parseInt(age) > 10) {  
            return mapping.findForward("success");  
        } else {  
            request.setAttribute("message", "用户名或密码错误！");  
            return mapping.findForward("fail");  
        }  
    }  
}
