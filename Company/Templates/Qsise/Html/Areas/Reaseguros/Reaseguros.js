<!--Js Local-->
$('.client__row').click((evt) => {
  const reinsuranceId = evt.currentTarget.dataset.reinsuranceId;
  $helpers.setVariable('glb_reinsuranceId', endorsementId);
  $helpers.open(1);
});
