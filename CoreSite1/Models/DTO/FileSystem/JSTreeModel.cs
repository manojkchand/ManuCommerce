using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class JsTreeModel
{
    public string data;
    public CoreSite1.Models.DTO.PageCategory attr;
    public string state = "open";
    public List<JsTreeModel> children;
   
}

public class JsTreeAttribute
{
    public string id;

 }