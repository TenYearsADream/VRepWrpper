
namespace VREPRemoteAPI.Enums
{
    public enum SimIK
    {  
        PseudoInverseMethod = 0,
        DampedLeastSquaresMethod = 1,
        XConstraint = 1,
        JacobianTransposeMethod = 2,
        YConstraint = 2,
        ZConstraint = 4,
        AlphaBetaConstraint = 8,
        GammaConstraint = 16,
        AvoidanceConstraint = 64
    }
}
