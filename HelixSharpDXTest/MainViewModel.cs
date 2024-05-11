namespace HelixSharpDXTest
{
    using System.Linq;


    using HelixToolkit.Wpf.SharpDX;
    using HelixToolkit.Wpf.SharpDX.Core;

    using SharpDX;
    using Media3D = System.Windows.Media.Media3D;
    using Point3D = System.Windows.Media.Media3D.Point3D;
    using Vector3D = System.Windows.Media.Media3D.Vector3D;
    using Transform3D = System.Windows.Media.Media3D.Transform3D;
    using Color = System.Windows.Media.Color;
    using Plane = SharpDX.Plane;
    using Vector3 = SharpDX.Vector3;
    using Colors = System.Windows.Media.Colors;
    using Color4 = SharpDX.Color4;
    using HelixToolkit.Wpf.SharpDX.Extensions;
    using SharpDX.Direct3D11;
    using System.IO;

    public class MainViewModel : BaseViewModel
    {
        public MeshGeometry3D Model { get; private set; }

        public Transform3D ModelTransform { get; private set; }

        public PhongMaterial RedMaterial { get; private set; }

        public Vector3D DirectionalLightDirection { get; private set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }

        public Vector3D UpDirection { set; get; } = new Vector3D(0, 1, 0);

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();

            // camera setup
            Camera = new PerspectiveCamera
            {
                Position = new Point3D(3, 3, 5),
                LookDirection = new Vector3D(-3, -3, -5),
                UpDirection = new Vector3D(0, 1, 0),
                FarPlaneDistance = 50000
            };

            // setup lighting            
            AmbientLightColor = Colors.DimGray;
            DirectionalLightColor = Colors.White;
            DirectionalLightDirection = new Vector3D(-2, -5, -2);

            // scene model3d
            MeshBuilder b1 = new MeshBuilder();
            b1.AddSphere(new Vector3(0, 0, 0), 0.5);
            b1.AddBox(new Vector3(0, 0, 0), 1, 0.5, 2, BoxFaces.All);

            MeshGeometry3D meshGeometry = b1.ToMeshGeometry3D();
            meshGeometry.Colors = new Color4Collection(meshGeometry.TextureCoordinates.Select(x => x.ToColor4()));
            Model = meshGeometry;

            List<MeshGeometry3D?> models = LoadStl($@"{Directory.GetCurrentDirectory()}\Models\shrigma.stl").Select(x => x.Geometry as MeshGeometry3D).ToList();
            
            if(models.First() != null)
            {
                Model = models.First();
            }
            else
            {
                throw new Exception("3D Model Null!");
            }

            ModelTransform = new Media3D.TranslateTransform3D(0, 0, 0);

            RedMaterial = PhongMaterials.Red;
        }

        public List<Object3D> LoadStl(string modelPath)
        {
            var reader = new StLReader();
            var list = reader.Read(modelPath);
            return list;
        }
    }
}
