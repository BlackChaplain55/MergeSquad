using System.Collections.ObjectModel;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [field: SerializeReference] public ObservableCollection<Unit> Units { get; private set; }
}
