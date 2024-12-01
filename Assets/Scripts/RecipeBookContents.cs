using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RecipeBookContents : MonoBehaviour
{
    [TextArea(10,20)]
    [SerializeField] private string content;
    [Space]
    [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space]
    [SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;


    private void OnValidate() {
        UpdatePagination();
        
        if(leftSide.text == content)
            return;


        SetupContent();
        
    }


    private void Awake() {
        SetupContent();
        UpdatePagination();
        
    }


    private void SetupContent() {
        leftSide.text = content;
        rightSide.text = content;
    }

    private void UpdatePagination(){
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();

    }

    public void PreviousPage(){
        if (leftSide.pageToDisplay > 1) {
            //bookContentAS.PlayOneShot(pageTurnSFX);
            AudioManager.Instance.PlaySFX("PageTurn");
        }

        if(leftSide.pageToDisplay < 1) {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay -2 > 1) {
            leftSide.pageToDisplay -= 2;
            //bookContentAS.PlayOneShot(pageTurnSFX);

        } else {
            leftSide.pageToDisplay = 1;
            //bookContentAS.PlayOneShot(pageTurnSFX);
        }


        rightSide.pageToDisplay = leftSide.pageToDisplay+1;
        UpdatePagination();
    }

    public void NextPage(){
        if(rightSide.pageToDisplay >= rightSide.textInfo.pageCount) {
            return;
        }

        if(leftSide.pageToDisplay >= leftSide.textInfo.pageCount -1) {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
            //bookContentAS.PlayOneShot(pageTurnSFX);
            AudioManager.Instance.PlaySFX("PageTurn");

        } else {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
            //bookContentAS.PlayOneShot(pageTurnSFX);
            AudioManager.Instance.PlaySFX("PageTurn");

        }

        UpdatePagination();
    }



}
