using UnityEngine;

namespace Services
{
	public static class LayerService
	{
		public static bool CheckLayersEquality(LayerMask objectLayer, LayerMask requiredLayer) => ((1 << objectLayer) & requiredLayer) > 0;
	}
}