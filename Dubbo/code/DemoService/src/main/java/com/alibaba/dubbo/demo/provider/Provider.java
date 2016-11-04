package com.alibaba.dubbo.demo.provider;

import java.io.IOException;

import org.springframework.context.support.ClassPathXmlApplicationContext;

public class Provider {

	public static void main(String[] args) throws Exception {
		// TODO Auto-generated method stub
		ClassPathXmlApplicationContext context = new ClassPathXmlApplicationContext(new String[] {"http://192.168.12.44/temp/provider.xml"});
        context.start();
        System.out.println("任意键关闭服务。。。");
        System.in.read(); // 按任意键退出	
	}

}
