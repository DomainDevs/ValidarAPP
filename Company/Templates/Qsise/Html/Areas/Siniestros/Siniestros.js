<!--Js Local-->
$('.client__row').click((evt) => {
  const claimid = evt.currentTarget.dataset.claimid;
  $helpers.setVariable('glb_claimId', claimid);
  $helpers.open(1);
});