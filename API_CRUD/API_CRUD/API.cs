using Nancy;
using NPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Taules
{
    public class PostUser
    {
        public string Email;
        public string Username;
        public string Password;
    }
    public class API : Nancy.NancyModule
    {
        public API() : base("/")
        {
            //Get Base.
            Get["/"] = parameters =>
            {
                return "<h1>CRUD</h1><br><h2>Taules:</h2><br>1. Client<br>2. Usuari<br>3. Oportunitat<br>" +
                "4. Estat";
            };
            var connexio = @"Data Source=192.168.100.11;Initial Catalog=FlyPath;User ID=sa;Password=123";
            //Custom error 500, InternalServerError.
            Response my500 = new Response
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            //Custom error 404, NotFound.
            Response my404 = new Response
            {
                StatusCode = HttpStatusCode.NotFound
            };

            //Custom error 400, BadRequest.
            Response my400 = new Response
            {
                StatusCode = HttpStatusCode.BadRequest
            };


            //GET : For fetching data and in search functionality.
            //POST : For adding data
            //PUT : For updating data
            //DELETE : For Deleting data

            //////////////////////////////////USUARIS////////////////////////////////////

            //Get tots els usuaris.
            Get["usuaris"] = parameters =>
                {
                    String a = "ad";
                    try
                    {
                        using (NPoco.Database myCon = new NPoco.Database(connexio, NPoco.DatabaseType.SqlServer2012))
                        {
                            var myData = myCon.Fetch<Usuari>();
                            String llista = "<h1>Llista: </h1><br>";
                            foreach (var item in myData)
                            {
                                llista += item + "<br>";
                            }
                            return fastJSON.JSON.ToJSON(myData);
                        }
                    }
                    catch (Exception)
                    {
                        return my500;
                    }
                };

            //Get un usuari amb el seu codi.
            Get["usuari/{codi}"] = parameters =>
            {
                try
                {
                    string _myUser = parameters.codi;

                    using (NPoco.Database myCon = new NPoco.Database(connexio, NPoco.DatabaseType.SqlServer2012))
                    {
                        if (myCon.Exists<Usuari>(_myUser))
                        {
                            var myData = myCon.Fetch<Usuari>(NPoco.Sql.Builder.Where("id=@0", _myUser));
                            String llista = "<h1>Llista: </h1><br>";
                            foreach (var item in myData)
                            {
                                llista += item + "<br>";
                            }
                            return llista;//fastJSON.JSON.ToJSON(myData);
                        }
                        else
                        {
                            return my400;
                        }
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };
            //Get Login
            Get["usuariLogin/{Username}/{Password}"] = parameters =>
            {
                try
                {
                    string myUsername = parameters.Username;
                    string myPassword = parameters.Password;

                    //string myUsername = this.Request.Form["Username"];
                    //string myPassword = this.Request.Form["Password"];

                    using (NPoco.Database myCon = new NPoco.Database(connexio, NPoco.DatabaseType.SqlServer2012))
                    {
                        if (myUsername!="" && myPassword!="" && myUsername != null && myPassword != null)
                        {
                            var myData = myCon.Single<Usuari>(NPoco.Sql.Builder.Where("Username=@0 AND Password=@1",myUsername, myPassword));
                           /* String llista="";
                            foreach (var item in myData)
                            {
                                llista += item + " ";
                            }*/
                            return fastJSON.JSON.ToJSON(myData);
                        }
                        else
                        {
                            return my400;
                        }
                    }
                }
                catch (Exception e)
                {
                    return my500;
                }
            };


            //Post d'un usuari.
            Post["usuariRegistre"] = parameters =>
            {
                try
                {
                    String s = "prova";
                    //PostUser registre = fastJSON.JSON.ToObject<PostUser>(this.Request.Form["registre"]);
                   
                    string myEmail = this.Request.Form["Email"];
                    string myUsername = this.Request.Form["Username"];
                    string myPassowrd = this.Request.Form["Password"];
                    

                    using (NPoco.Database myCon = new NPoco.Database(connexio, NPoco.DatabaseType.SqlServer2012))
                    {
                        var myNouReg = new Usuari
                        {
                            Email = myEmail,
                            Username = myUsername,
                            Password = myPassowrd
                        };

                        myCon.Insert(myNouReg);
                        return myNouReg;
                    }

                }
                catch (Exception e)
                {
                    string s = "asd";
                    return my500;
                }
            };

            //Put un client.

            Put["usuariPU/{codi}"] = parameters =>
             {
                 try
                 {
                     string myID = parameters.codi;
                     string myEmail = this.Request.Form["Email"];
                     string myNom = this.Request.Form["Nom"];
                     string myPassword = this.Request.Form["Password"];

                     using (NPoco.Database myCon = new NPoco.Database(connexio, NPoco.DatabaseType.SqlServer2012))
                    { 
                        var myNouReg = myCon.Fetch<Usuari>(NPoco.Sql.Builder.Where("ID=@0", myID));

                       
                            myNouReg[0].Username = myNom;
                        
                       
                            myNouReg[0].Email = myEmail;
                        
                       
                            myNouReg[0].Password = myPassword;
                        

                        myCon.Update(myNouReg[0]);

                        var myData = myCon.Fetch<Usuari>(NPoco.Sql.Builder.Where("id=@0", myID));
                        var result = new Usuari();

                        foreach (var item in myData)
                        {
                            result = item;
                        }
                        return result;
                           
                    }
                    
                 }
                 catch (Exception)
                 {
                     return my500;
                 }
             };

            //Delete un usuari.
            Delete["usuariD/{codi}"] = parameters =>
            {
                try
                {
                    string myUser = parameters.codi;

                    using (NPoco.Database myCon = new NPoco.Database(connexio, NPoco.DatabaseType.SqlServer2012))
                    {
                        var myNouReg = myCon.Fetch<Usuari>(NPoco.Sql.Builder.Where("id=@0", myUser));

                        myCon.Delete(myNouReg[0]);
                        return "";
                    }
                }
                catch (Exception)
                {
                    return my500;
                }

            };
            //////////////////////////////////USUARI////////////////////////////////////


            //////////////////////////////////OPORTUNITATS////////////////////////////////////

            /*
            //?user=Usuari3
            //Get totes les oportunitats.
            Get["oportunitat"] = parameters =>
            {
                var user = "";
                if (this.Request.Query["user"] != null)
                {
                    user = this.Request.Query["user"];
                }

                try
                {
                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {
                        var myData = new List<Oportunitat>();
                        if (user != "")
                        {
                            myData = myCon.Fetch<Oportunitat>("WHERE Usuari_ID=@0", user);
                        }
                        else
                        {
                            myData = myCon.Fetch<Oportunitat>("WHERE acabat like 0");
                        }

                        if (myData.Count > 1)
                        {
                            return fastJSON.JSON.ToJSON(myData);
                        }
                        else
                        {
                            return my404;
                        }
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };

            //Get una oporutunitat.

            Get["oportunitat/{codi}"] = parameters =>
            {
                try
                {
                    string myOp = parameters.codi;

                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {
                        var myData = myCon.Single<Oportunitat>(NPoco.Sql.Builder.Where("id=@0", myOp));
                        return fastJSON.JSON.ToJSON(myData);

                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };

            //Post una Oportunitat.
            Post["oportunitat/{codi}"] = parameters =>
            {
                try
                {
                    string myUserID = parameters.codi;
                    PostUser pepito = fastJSON.JSON.ToObject<PostUser>(this.Request.Form["pepito"]);

                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {

                        if (myCon.Fetch<Usuari>(NPoco.Sql.Builder.Where("ID=@0", myUserID)).Any())
                        {
                            var myData = myCon.Fetch<Oportunitat>();
                            var myNouReg = new Oportunitat();

                            myNouReg = new Oportunitat
                            {
                                Nom = pepito.Nom,
                                Usuari_ID = myUserID,
                                Data_Inici = DateTime.Today,
                            };
                            if (!myCon.Fetch<Oportunitat>(NPoco.Sql.Builder.Where("Nom=@0 and Client_ID=@1 and Estat=@2", pepito.Nom)).Any())
                            {
                                myCon.Insert(myNouReg);
                                return myNouReg;
                            }
                            else
                            {
                                return my404;
                            }
                        }
                        else
                        {
                            return my404;
                        }
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };

            //Put una Oportunitat.
            Put["oportunitat/{usuari}/{codi}"] = parameters =>
             {
                 try
                 {
                     string myID = parameters.codi;
                     string myUserID = parameters.usuari;
                     PostUser pepito = fastJSON.JSON.ToObject<PostUser>(this.Request.Form["pepito"]);

                     using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                     {
                         var myNouReg = myCon.Fetch<Oportunitat>(NPoco.Sql.Builder.Where("id=@0", myID));

                         if (pepito.Nom != "" && pepito.Nom != null && pepito.Nom != myNouReg[0].Nom)
                         {
                             myNouReg[0].Nom = pepito.Nom;
                         }
                       
                         myCon.Update(myNouReg[0]);

                         myNouReg = myCon.Fetch<Oportunitat>(NPoco.Sql.Builder.Where("id=@0", myID));
                         var result = new Oportunitat();

                         foreach (var item in myNouReg)
                         {
                             result = item;
                         }
                         return result;
                     }
                 }
                 catch (Exception)
                 {
                     return my500;
                 }
             };

            //Delete una oportunitat
            Delete["oportunitat/{codi}"] = parameters =>
            {
                try
                {
                    string myID = parameters.codi;

                    if (Int32.TryParse(myID, out int myID2))
                    {
                        using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                        {
                            if (myCon.Exists<Oportunitat>(myID))
                            {
                                var myNouReg = myCon.Fetch<Oportunitat>(NPoco.Sql.Builder.Where("id=@0", myID));

                                myCon.Delete(myNouReg[0]);
                                return "";
                            }
                            else
                            {
                                return my404;
                            }
                        }
                    }
                    else
                    {
                        return my400;
                    }
                }
                catch (Exception)
                {
                    return my500;
                }

            };

            //////////////////////////////////ESTATS////////////////////////////////////

            //Get número d'estats.
            Get["estat/count"] = parameters =>
            {
                try
                {
                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {
                        var myData = myCon.Fetch<Estat>();
                        return myData.Count() + "";
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };

            //Get tots els estats.
            Get["estat"] = parameters =>
            {
                try
                {
                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {
                        var myData = myCon.Fetch<Estat>();
                        if (myData.Count > 1)
                        {
                            return fastJSON.JSON.ToJSON(myData);
                        }
                        else
                        {
                            return my404;
                        }
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };

            //Get un estat.
            Get["estat/{codi}"] = parameters =>
            {
                try
                {
                    string myEstat = parameters.codi;

                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {
                        var myData = myCon.Fetch<Estat>(NPoco.Sql.Builder.Where("id=@0", myEstat));
                        return fastJSON.JSON.ToJSON(myData);
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };

            //Post un estat.
            Post["estat"] = parameters =>
                {
                    try
                    {
                        string myNom = this.Request.Form["Nom"];

                        using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                        {
                            if (myNom != "")
                            {
                                var myNouReg = new Estat
                                {
                                    Nom = myNom
                                };

                                myCon.Insert(myNouReg);
                                return myNouReg;
                            }
                            else
                            {
                                return my400;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return my500;
                    }
                };

            //Put un estat.
            Put["estat/{codi}"] = parameters =>
            {
                try
                {
                    string myID = parameters.codi;
                    string myNom = this.Request.Form["Nom"];

                    if (Int32.TryParse(myID, out int myID2))
                    {
                        using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                        {
                            if (myCon.Exists<Estat>(myID2))
                            {
                                var myNouReg = myCon.Fetch<Estat>(NPoco.Sql.Builder.Where("id=@0", myID2));

                                if (myNom != myNouReg[0].Nom && myNom != "")
                                {
                                    myNouReg[0].Nom = myNom;
                                    myCon.Update(myNouReg[0]);

                                    return myNouReg[0];
                                }
                                else
                                {
                                    return my400;
                                }
                            }
                            else
                            {
                                return my404;
                            }
                        }
                    }
                    else
                    {
                        return my400;
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };


            //Delete un Estat.
            Delete["estat/{codi}"] = parameters =>
            {
                try
                {
                    int myID = parameters.codi;

                    using (NPoco.Database myCon = new NPoco.Database(@"Data Source=192.168.110.79;Initial Catalog=oportunitats;User ID=sa;Password=", NPoco.DatabaseType.SqlServer2012))
                    {
                        if (!myCon.Fetch<Oportunitat>("WHERE Estat=@0", myID).Any())
                        {
                            if (myCon.Exists<Estat>(myID))
                            {
                                var myNouReg = myCon.Fetch<Estat>(NPoco.Sql.Builder.Where("id=@0", myID));

                                myCon.Delete(myNouReg[0]);
                                return "";
                            }
                            else
                            {
                                return my404;
                            }
                        }
                        else
                        {
                            return my400;
                        }
                    }
                }
                catch (Exception)
                {
                    return my500;
                }
            };
            */


        }
        public bool comprovarUserLogin(String username, String password)
        {

            return false;
        }
    }
}
