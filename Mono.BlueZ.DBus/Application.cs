using System.Collections.Generic;

using DBus;
using org.freedesktop.DBus;

namespace Mono.BlueZ
{
   public class Application : ObjectManager
   {
      private readonly Bus bus;
      private string path = "/";
      private List<Service> services;

      public event InterfacesAddedHandler InterfacesAdded;
      public event InterfacesRemovedHandler InterfacesRemoved;

      public Application(Bus bus)
      {
         this.bus = bus;
         services = new List<Service>();
         bus.Register(new ObjectPath(path), this);
      }

      public ObjectPath GetPath()  
      {
         return new ObjectPath(path);
      }

      public void AddService(Service service)
      {
         services.Add(service);
      }

      [return: Argument("objects")]
      public IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>> GetManagedObjects()
      {
         var manageObjets = new Dictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>();

         foreach (var service in services)
         {
            manageObjets.Add(service.GetPath(), service.GetProperties());
            var characteristics = service.GetCharacteristics();

            foreach (var characteristic in characteristics)
            {
               manageObjets.Add(characteristic.GetPath(), characteristic.GetProperties());
               var descriptors = characteristic.GetDescriptors();

               foreach (var descriptor in descriptors)
               {
                  manageObjets.Add(descriptor.GetPath(), descriptor.GetProperties());
               }
            }
         }

         return manageObjets;
      }
   }
}
