using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for PackageGroupCollection
/// </summary>
/// 

public class PackageGroupCollection : System.Collections.CollectionBase
{
  public PackageGroupCollection()
  {
  }

  public void Add(PackageGroup aPackageGroup)
  {
    List.Add(aPackageGroup);
  }

  public void Remove(int index)
  {
    if (index > Count - 1 || index < 0)
    {
      throw new Exception("Index not valid!");
    }
    else
    {
      List.RemoveAt(index);
    }
  }

  public PackageGroup Item(int Index)
  {
    return (PackageGroup)List[Index];
  }
}
